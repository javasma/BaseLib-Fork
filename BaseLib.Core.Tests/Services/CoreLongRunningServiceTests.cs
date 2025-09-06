using BaseLib.Core.Extensions;
using BaseLib.Core.Services;
using Moq;
using Xunit;

namespace BaseLib.Core.Tests.Services
{
    public partial class CoreLongRunningServiceTests
    {
        [Fact]
        public async Task RunAsync_ReturnsSuspended()
        {
            // Arrange
            var fireOnlyMock = new Mock<ICoreServiceFireOnly>();
            var stateStore = new InMemoryStateStore();

            var service = new LongRunningMasterService(
                fireOnlyMock.Object,
                stateStore);

            var request = new LongRunningMasterRequest { NumberOfChildren = 5 };

            // Act
            var response = await service.RunAsync(request);

            // Assert
            Assert.True(response.Succeeded);
            Assert.True(response.ReasonCode.HasFlag(CoreServiceReasonCode.Suspended));
            Assert.Equal(request.NumberOfChildren, response.NumberOfChildrenFired);
        }

        [Fact]
        public async Task RunAsync_ReturnsNotSuspended()
        {
            // Arrange
            var fireOnlyMock = new Mock<ICoreServiceFireOnly>();
            var stateStore = new InMemoryStateStore();

            var service = new LongRunningMasterService(
                fireOnlyMock.Object,
                stateStore);

            var request = new LongRunningMasterRequest { NumberOfChildren = 0 };

            // Act
            var response = await service.RunAsync(request);

            // Assert
            Assert.True(response.Succeeded);
            Assert.False(response.ReasonCode.HasFlag(CoreServiceReasonCode.Suspended));
        }

        [Fact]
        public async Task RunAsync_ResumeReturnsSucceeded()
        {
            // Arrange
            var fireOnlyMock = new Mock<ICoreServiceFireOnly>();
            var stateStore = new InMemoryStateStore();

            // Create the request
            var request = new LongRunningMasterRequest { NumberOfChildren = 5 };

            // Create a new instance of the service to simulate a fresh start
            var service = new LongRunningMasterService(fireOnlyMock.Object, stateStore);

            // Act
            var response = await service.RunAsync(request);

            // Assert
            Assert.True(response.Succeeded);
            Assert.True(response.ReasonCode.HasFlag(CoreServiceReasonCode.Suspended));
            Assert.Equal(request.NumberOfChildren, response.NumberOfChildrenFired);

            // Simulate resuming the service

            //create a new instance of the service to simulate a fresh start
            var resumingService = new LongRunningMasterService(
                fireOnlyMock.Object,
                stateStore) as ICoreLongRunningService;

            var resumeResponse = await resumingService.ResumeAsync(response.OperationId!);

            // Assert resume succeeded
            Assert.True(resumeResponse.Succeeded);
            // Reason code also must be succeeded
            Assert.True(resumeResponse.ReasonCode.HasFlag(CoreServiceReasonCode.Succeeded));
            // Must not be suspended
            Assert.False(resumeResponse.ReasonCode.HasFlag(CoreServiceReasonCode.Suspended));
            //this is retrieved from the state store
            //Assert.Equal(request.NumberOfChildren, resumeResponse.NumberOfChildrenFired);
        }
    }
    
}

