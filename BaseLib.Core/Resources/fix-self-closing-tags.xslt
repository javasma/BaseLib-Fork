<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ds="http://www.w3.org/2000/09/xmldsig#"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
    exclude-result-prefixes="ds ms"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
      <xsl:if test=".=''">
        <xsl:comment>prevent self closing tag</xsl:comment>
      </xsl:if>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="ds:Signature">
    <xsl:copy-of select="."/>
  </xsl:template>

	<xsl:template match="cbc:UBLVersionID">
		<cbc:UBLVersionID>
			<xsl:value-of select="."/>
		</cbc:UBLVersionID>
	</xsl:template>
	
  
</xsl:stylesheet>
