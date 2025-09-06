using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Core.Services;

namespace BaseLib.Core.Tests.Services;

/// <summary>
/// In-memory implementation of ICoreServiceStateStore that preserves type information
/// for proper deserialization of complex objects and primitive types.
/// </summary>
public class InMemoryStateStore : ICoreServiceStateStore
{
    // JsonSerializerOptions instances should be reused for better performance
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
    
    // Static ConcurrentDictionary simulating a file store with string values
    private static readonly ConcurrentDictionary<string, string> _cache = new();

    /// <summary>
    /// Reads the stored state for a specific operation
    /// </summary>
    /// <param name="operationId">The operation identifier</param>
    /// <returns>A dictionary containing the state values with their original types</returns>
    public async Task<IDictionary<string, object?>> ReadAsync(string operationId)
    {      
        // Try to get the state for the operation
        if (_cache.TryGetValue(operationId, out var serializedState) && !string.IsNullOrEmpty(serializedState))
        {
            // Simulate async operation
            await Task.CompletedTask;

            // Deserialize the state dictionary with typed values
            var typedState = JsonSerializer.Deserialize<Dictionary<string, TypedValue>>(
                serializedState,
                jsonOptions
            ) ?? throw new InvalidOperationException("Failed to deserialize state from store");

            return typedState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.GetValue()
            );
        }

        // Return empty dictionary when no state is found
        return new Dictionary<string, object?>(0);
    }

    /// <summary>
    /// Stores the state for a specific operation with type information preserved
    /// </summary>
    /// <param name="operationId">The operation identifier</param>
    /// <param name="state">The state to store</param>
    public Task WriteAsync(string operationId, IDictionary<string, object?> state)
    {

        // Create a dictionary with typed values
        var typedStateDict = new Dictionary<string, TypedValue>(state.Count);
        
        // Store each value with its type information
        foreach (var kvp in state)
        {
            typedStateDict[kvp.Key] = new TypedValue(kvp.Value);
        }
        
        // Serialize the entire typed state dictionary to a single JSON string
        string serializedState = JsonSerializer.Serialize(typedStateDict, jsonOptions);
        
        // Store the serialized state in the cache
        _cache[operationId] = serializedState;
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Represents a value with its type information to ensure proper deserialization
    /// </summary>
    private struct TypedValue
    {
        // Public properties for serialization
        public string? TypeName { get; set; }
        public string? ValueJson { get; set; }
        
        // Constructor to create from an object
        public TypedValue(object? value)
        {
            if (value == null)
            {
                TypeName = null;
                ValueJson = null;
                return;
            }
            
            // Store the type's assembly qualified name for precise reconstruction
            TypeName = value.GetType().AssemblyQualifiedName;
            
            // Serialize the value using the shared options
            ValueJson = JsonSerializer.Serialize(value, jsonOptions);
        }
        
        public readonly object? GetValue()
        {
            // Fast path for null values
            if (TypeName == null || ValueJson == null)
            {
                return null;
            }
            
            try
            {
                // Get the type from the stored type name
                var type = Type.GetType(TypeName);
                if (type == null)
                {
                    return null;
                }
                
                // Deserialize using the original type
                return JsonSerializer.Deserialize(ValueJson, type, jsonOptions);
            }
            catch (Exception)
            {
                // If deserialization fails, return null
                return null;
            }
        }
    }
}
