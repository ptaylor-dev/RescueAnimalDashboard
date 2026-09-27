using Sorting;

namespace TestSorting
{
    public class SortingTests
    {
        [Fact]
        public void TestMergeSortEmptyList()
        {
            var sorter = new MergeSorter();

            // Create empty list
            List<CustomDataStructure> testData = [];

            var results = sorter.Sort(testData, item => item.String1);

            Assert.Empty(results);
        }

        [Fact]
        public void TestMergeSortAscendingOfSortedData()
        {
            var sorter = new MergeSorter();

            List<CustomDataStructure> testData = [];

            // Create a list of test data
            testData.Add(new CustomDataStructure
            {
                String1 = "Apple",
                String2 = "Snake"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Banana",
                String2 = "Rat"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Celery",
                String2 = "Lion"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "DragonFruit",
                String2 = "Puppy"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Eggplant",
                String2 = "Tiger"
            });

            var results = sorter.Sort(testData, item => item.String1);

            Assert.Collection(results,
                item1 => Assert.Equal("Apple", item1.String1),
                item2 => Assert.Equal("Banana", item2.String1),
                item3 => Assert.Equal("Celery", item3.String1),
                item4 => Assert.Equal("DragonFruit", item4.String1),
                item5 => Assert.Equal("Eggplant", item5.String1)
                );
        }

        [Fact]
        public void TestMergeSortAscendingOfUnsortedData()
        {
            var sorter = new MergeSorter();

            // Create a list of test data
            List<CustomDataStructure> testData = [];

            testData.Add(new CustomDataStructure
            {
                String1 = "Apple",
                String2 = "Snake"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Banana",
                String2 = "Rat"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Celery",
                String2 = "Lion"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "DragonFruit",
                String2 = "Puppy"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Eggplant",
                String2 = "Tiger"
            });

            var result = sorter.Sort(testData, item => item.String2);

            Assert.Collection(result,
                item1 =>
                {
                    Assert.Equal("Celery", item1.String1);
                    Assert.Equal("Lion", item1.String2);
                },
                item2 =>
                {
                    Assert.Equal("DragonFruit", item2.String1);
                    Assert.Equal("Puppy", item2.String2);
                },
                item3 =>
                {
                    Assert.Equal("Banana", item3.String1);
                    Assert.Equal("Rat", item3.String2);
                },
                item4 =>
                {
                    Assert.Equal("Apple", item4.String1);
                    Assert.Equal("Snake", item4.String2);
                },
                item5 =>
                {
                    Assert.Equal("Eggplant", item5.String1);
                    Assert.Equal("Tiger", item5.String2);
                }
                );
        }

        [Fact]
        public void TestMergeSortDescendingOfUnsortedData()
        {
            var sorter = new MergeSorter();

            // Create a list of test data
            List<CustomDataStructure> testData = [];

            testData.Add(new CustomDataStructure
            {
                String1 = "Apple",
                String2 = "Snake"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Banana",
                String2 = "Rat"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Celery",
                String2 = "Lion"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "DragonFruit",
                String2 = "Puppy"
            });
            testData.Add(new CustomDataStructure
            {
                String1 = "Eggplant",
                String2 = "Tiger"
            });

            // Sort data descending
            var result = sorter.Sort(testData, item => item.String2, false);

            Assert.Collection(result,
                item1 =>
                {
                    Assert.Equal("Eggplant", item1.String1);
                    Assert.Equal("Tiger", item1.String2);
                },
                item2 =>
                {
                    Assert.Equal("Apple", item2.String1);
                    Assert.Equal("Snake", item2.String2);
                },
                item3 =>
                {
                    Assert.Equal("Banana", item3.String1);
                    Assert.Equal("Rat", item3.String2);
                },
                item4 =>
                {
                    Assert.Equal("DragonFruit", item4.String1);
                    Assert.Equal("Puppy", item4.String2);
                },
                item5 =>
                {
                    Assert.Equal("Celery", item5.String1);
                    Assert.Equal("Lion", item5.String2);
                }
                );
        }

        [Fact]
        public void TestMergeSortMultiSortedData()
        {
            var sorter = new MergeSorter();

            // Create a list of test data
            List<CustomDataStructure> testData = [];

            testData.Add(new CustomDataStructure { Num1 = 1, Num2 = 3 });
            testData.Add(new CustomDataStructure { Num1 = 2, Num2 = 2 });
            testData.Add(new CustomDataStructure { Num1 = 1, Num2 = 2 });
            testData.Add(new CustomDataStructure { Num1 = 2, Num2 = 1 });
            testData.Add(new CustomDataStructure { Num1 = 1, Num2 = 1 });
            testData.Add(new CustomDataStructure { Num1 = 2, Num2 = 3 });

            // Sort data by second parameter first
            var firstSort = sorter.Sort(testData, item => item.Num2);

            // Sort data by first parameter
            var result = sorter.Sort(firstSort, item => item.Num1);


            Assert.Collection(result,
                item1 =>
                {
                    Assert.Equal(1, item1.Num1);
                    Assert.Equal(1, item1.Num2);
                },
                item2 =>
                {
                    Assert.Equal(1, item2.Num1);
                    Assert.Equal(2, item2.Num2);
                },
                item3 =>
                {
                    Assert.Equal(1, item3.Num1);
                    Assert.Equal(3, item3.Num2);
                },
                item4 =>
                {
                    Assert.Equal(2, item4.Num1);
                    Assert.Equal(1, item4.Num2);
                },
                item5 =>
                {
                    Assert.Equal(2, item5.Num1);
                    Assert.Equal(2, item5.Num2);
                },
                item6 =>
                {
                    Assert.Equal(2, item6.Num1);
                    Assert.Equal(3, item6.Num2);
                });
        }

        [Fact]
        public void TestMergeSortMultiSortedDifferentDirectionsData()
        {
            var sorter = new MergeSorter();

            // Create a list of test data
            List<CustomDataStructure> testData = [];

            testData.Add(new CustomDataStructure { Num1 = 1, Num2 = 3 });
            testData.Add(new CustomDataStructure { Num1 = 2, Num2 = 2 });
            testData.Add(new CustomDataStructure { Num1 = 1, Num2 = 2 });
            testData.Add(new CustomDataStructure { Num1 = 2, Num2 = 1 });
            testData.Add(new CustomDataStructure { Num1 = 1, Num2 = 1 });
            testData.Add(new CustomDataStructure { Num1 = 2, Num2 = 3 });

            // Sort data by second parameter first
            var firstSort = sorter.Sort(testData, item => item.Num2);

            // Sort data by first parameter
            var result = sorter.Sort(firstSort, item => item.Num1, false);


            Assert.Collection(result,
                item1 =>
                {
                    Assert.Equal(2, item1.Num1);
                    Assert.Equal(1, item1.Num2);
                },
                item2 =>
                {
                    Assert.Equal(2, item2.Num1);
                    Assert.Equal(2, item2.Num2);
                },
                item3 =>
                {
                    Assert.Equal(2, item3.Num1);
                    Assert.Equal(3, item3.Num2);
                },
                item4 =>
                {
                    Assert.Equal(1, item4.Num1);
                    Assert.Equal(1, item4.Num2);
                },
                item5 =>
                {
                    Assert.Equal(1, item5.Num1);
                    Assert.Equal(2, item5.Num2);
                },
                item6 =>
                {
                    Assert.Equal(1, item6.Num1);
                    Assert.Equal(3, item6.Num2);
                });
        }

        [Fact]
        public void TestMergeSortWithNullValues()
        {
            var sorter = new MergeSorter();

            // Create a list of test data
            List<CustomDataStructure> testData = [];

            testData.Add(new CustomDataStructure { Num1 = 1 });
            testData.Add(new CustomDataStructure { Num1 = null });
            testData.Add(new CustomDataStructure { Num1 = 4 });
            testData.Add(new CustomDataStructure { Num1 = null });
            testData.Add(new CustomDataStructure { Num1 = 3 });
            testData.Add(new CustomDataStructure { Num1 = 2 });

            // Sort data
            var result = sorter.Sort(testData, item => item.Num1);

            Assert.Collection(result,
                item1 =>
                {
                    Assert.Null(item1.Num1);
                },
                item2 =>
                {
                    Assert.Null(item2.Num1);
                },
                item3 =>
                {
                    Assert.Equal(1, item3.Num1);
                },
                item4 =>
                {
                    Assert.Equal(2, item4.Num1);
                },
                item5 =>
                {
                    Assert.Equal(3, item5.Num1);
                },
                item6 =>
                {
                    Assert.Equal(4, item6.Num1);
                });
        }
    }

    internal struct CustomDataStructure
    {
        internal string String1;
        internal string String2;
        internal int? Num1;
        internal int? Num2;
    }
}
