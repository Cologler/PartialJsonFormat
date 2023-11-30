namespace PartialJsonFormat.Tests;

[TestClass]
public class PartialJsonFormaterTests
{
    static string Normalize(string s) => s.Replace(" ", "").Replace("\r", "").Replace("\n", "");

    [TestMethod]
    public void TestFormatJson_CompletedJson()
    {
        var formater = new PartialJsonFormater();
        string inputJson = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";

        // Act
        string result = formater.FormatJson(inputJson);

        // Assert
        Assert.AreEqual("""
            {
               "name": "John",
               "age": 30,
               "city": "New York"
            }
            """, 
            result);

        Assert.AreEqual(Normalize(inputJson), Normalize(result));
    }

    [TestMethod]
    public void TestFormatJson_TruncatedJson()
    {
        var formater = new PartialJsonFormater();
        string inputJson = "{\"name\":\"John\",\"age\":30,\"ci";

        // Act
        string result = formater.FormatJson(inputJson);

        // Assert
        Assert.AreEqual("""
            {
               "name": "John",
               "age": 30,
               "ci
            """,
            result);

        Assert.AreEqual(Normalize(inputJson), Normalize(result));
    }

    [TestMethod]
    public void TestFormatJson_TruncatedJson_WithNestedObjects()
    {
        // Arrange
        var formater = new PartialJsonFormater();
        string inputJson = "{\"name\":\"John\",\"address\":{\"city\":\"New York\",\"zipcode\":10001},\"age\":30,\"ci";
        string result = formater.FormatJson(inputJson);

        Assert.AreEqual("""
            {
               "name": "John",
               "address": {
                  "city": "New York",
                  "zipcode": 10001
               },
               "age": 30,
               "ci
            """, result);

        Assert.AreEqual(Normalize(inputJson), Normalize(result));
    }

    [TestMethod]
    public void TestFormatJson_TruncatedJson_WithArrays()
    {
        // Arrange
        var formater = new PartialJsonFormater();
        string inputJson = "{\"numbers\":[1,2,3],\"names\":[\"John\",\"Jane\"],\"ci";

        // Act
        string result = formater.FormatJson(inputJson);

        // Assert
        Assert.AreEqual("""
            {
               "numbers": [
                  1,
                  2,
                  3
               ],
               "names": [
                  "John",
                  "Jane"
               ],
               "ci
            """, result);

        Assert.AreEqual(Normalize(inputJson), Normalize(result));
    }

    [TestMethod]
    public void TestFormatJson_TruncatedJson_WithDifferentDataTypes()
    {
        // Arrange
        var formater = new PartialJsonFormater();
        string inputJson = "{\"name\":\"John\",\"age\":30,\"isStudent\":true,\"grades\":{\"math\":90,\"history\":85},\"ci";

        // Act
        string result = formater.FormatJson(inputJson);

        // Assert
        Assert.AreEqual("""
            {
               "name": "John",
               "age": 30,
               "isStudent": true,
               "grades": {
                  "math": 90,
                  "history": 85
               },
               "ci
            """, result);

        Assert.AreEqual(Normalize(inputJson), Normalize(result));
    }

    [TestMethod]
    public void TestFormatJson_EmptyJson()
    {
        // Arrange
        var formater = new PartialJsonFormater();

        // Act
        string result = formater.FormatJson(string.Empty);

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void TestFormatJson_InvalidJson()
    {
        // Arrange
        var formater = new PartialJsonFormater();
        string invalidJson = "invalid json";

        // Act
        string result = formater.FormatJson(invalidJson);

        // Assert
        Assert.AreEqual(invalidJson, result);
    }
}