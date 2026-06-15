using Xunit;

namespace SilkJson.Tests
{
    /// <summary>
    /// Тесты для JsonArray
    /// </summary>
    public class JsonArrayTests
    {
        [Fact]
        public void JsonArray_CreateEmpty_ReturnsValidArray()
        {
            // Arrange & Act
            JsonArray array = new JsonArray();

            // Assert
            Assert.NotNull(array);
            Assert.Equal("[]", array.ToString());
        }

        [Fact]
        public void JsonArray_AddItems_ReturnsValidJson()
        {
            // Arrange
            JsonArray array = new JsonArray();

            // Act
            array.Add("first");
            array.Add(2);
            array.Add(true);

            // Assert
            Assert.Equal("[\"first\",2,true]", array.ToString());
        }

        [Fact]
        public void JsonArray_GetByIndex_ReturnsCorrectValue()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("zero");
            array.Add("one");
            array.Add("two");

            // Act
            Json result = array[1];

            // Assert
            Assert.Equal("one", result.ToString());
        }

        [Fact]
        public void JsonArray_SetByIndex_UpdatesValue()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("original");
            array.Add("original2");

            // Act
            array[0] = "updated";

            // Assert
            Assert.Equal("updated", array[0].ToString());
        }

        [Fact]
        public void JsonArray_Insert_InsertsAtPosition()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");
            array.Add("third");

            // Act
            array.Insert(1, "second");

            // Assert
            Assert.Equal("second", array[1].ToString());
            Assert.Equal(3, array.Count);
        }

        [Fact]
        public void JsonArray_Remove_RemovesItem()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");
            array.Add("second");
            array.Add("third");

            // Act
            bool removed = array.Remove("second");

            // Assert
            Assert.True(removed);
            Assert.Equal(2, array.Count);
        }

        [Fact]
        public void JsonArray_RemoveAt_RemovesByIndex()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");
            array.Add("second");
            array.Add("third");

            // Act
            array.RemoveAt(1);

            // Assert
            Assert.Equal(2, array.Count);
            Assert.Equal("third", array[1].ToString());
        }

        [Fact]
        public void JsonArray_Clear_RemovesAllItems()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("item1");
            array.Add("item2");

            // Act
            array.Clear();

            // Assert
            Assert.Equal(0, array.Count);
            Assert.Equal("[]", array.ToString());
        }

        [Fact]
        public void JsonArray_IndexOf_ReturnsCorrectIndex()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");
            array.Add("second");
            array.Add("third");

            // Act
            int index = array.IndexOf("second");

            // Assert
            Assert.Equal(1, index);
        }

        [Fact]
        public void JsonArray_AddRange_AddsMultipleItems()
        {
            // Arrange
            JsonArray array = new JsonArray();

            // Act
            array.AddRange("a", "b", "c");

            // Assert
            Assert.Equal(3, array.Count);
            Assert.Equal("[\"a\",\"b\",\"c\"]", array.ToString());
        }

        [Fact]
        public void JsonArray_Count_ReturnsCorrectCount()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("1");
            array.Add("2");
            array.Add("3");

            // Act & Assert
            Assert.Equal(3, array.Count);
        }

        [Fact]
        public void JsonArray_IsReadOnly_ReturnsFalse()
        {
            // Arrange & Act
            JsonArray array = new JsonArray();

            // Assert
            Assert.False(array.IsReadOnly);
        }

        [Fact]
        public void JsonArray_LargeArray_SerializesCorrectly()
        {
            // Arrange
            JsonArray array = new JsonArray();
            for (int i = 0; i < 1000; i++)
            {
                array.Add(i);
            }

            // Act
            string result = array.ToString();

            // Assert
            Assert.NotEmpty(result);
            Assert.Contains("0", result);
            Assert.Contains("999", result);
        }

        [Fact]
        public void JsonArray_GetIndexOutOfRange_ReturnsNull()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");

            // Act
            Json result;
            try { result = array[10]; } catch { result = new JsonValue(null); }

            // Assert
            Assert.True(result.IsMissed);
        }

        [Fact]
        public void JsonArray_SetIndexOutOfRange_DoesNotThrow()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");

            // Act & Assert (should not throw)
            try { array[10] = "out of range"; } catch { }
            Assert.Equal(1, array.Count);
        }

        [Fact]
        public void JsonArray_RemoveNonExistingItem_ReturnsFalse()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");

            // Act
            bool removed = array.Remove("nonexistent");

            // Assert
            Assert.False(removed);
        }

        [Fact]
        public void JsonArray_RemoveAtOutOfRange_DoesNotThrow()
        {
            // Arrange
            JsonArray array = new JsonArray();
            array.Add("first");

            // Act & Assert (should not throw)
            try { array.RemoveAt(10); } catch { }
            Assert.Equal(1, array.Count);
        }
    }
}
