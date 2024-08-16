using System;
using System.Collections.Generic;
using System.Linq;
using ASSLINQ;
namespace LINQ_Assignment01
{   class Program
    {   
        static void Main(string[] args)
        {
            #region LINQ - Restriction Operators

            Console.WriteLine("LINQ - Restriction Operators");
            Console.WriteLine("----------------------------");

            // 1. Find all products that are out of stock.
            var outOfStockProducts = ListGenerators.ProductList
                                                    .Where(p => p.UnitsInStock == 0)
                                                    .Select(p => p.ProductName);
            Console.WriteLine("1. Products out of stock:");
            Console.WriteLine(string.Join("\n- ", outOfStockProducts.Prepend("-")));

            // 2. Find all products that are in stock and cost more than 3.00 per unit.
            var inStockExpensiveProducts = ListGenerators.ProductList
                                                         .Where(p => p.UnitsInStock > 0 && p.UnitPrice > 3.00m)
                                                         .Select(p => $"- {p.ProductName} (Price: {p.UnitPrice:C}, Stock: {p.UnitsInStock})");
            Console.WriteLine("2. Products in stock and cost more than 3.00 per unit:");
            Console.WriteLine(string.Join("\n", inStockExpensiveProducts));

            // 3. Returns digits whose name is shorter than their value.
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var shortNamedDigits = digits
                                   .Where((name, index) => name.Length < index)
                                   .Select(name => $"- {name}");
            Console.WriteLine("3. Digits with names shorter than their value:");
            Console.WriteLine(string.Join("\n", shortNamedDigits));

            #endregion

            #region LINQ - Element Operators

            Console.WriteLine("LINQ - Element Operators");
            Console.WriteLine("------------------------");

            // 1. Get first Product out of Stock
            var firstOutOfStock = ListGenerators.ProductList.FirstOrDefault(p => p.UnitsInStock == 0)?.ProductName ?? "No products out of stock";
            Console.WriteLine($"1. First product out of stock: {firstOutOfStock}");

            // 2. Return the first product whose Price > 1000, unless there is no match, in which case null is returned.
            var expensiveProduct = ListGenerators.ProductList.FirstOrDefault(p => p.UnitPrice > 1000)?.ProductName ?? "No products found";
            Console.WriteLine($"2. First product with price > 1000: {expensiveProduct}");

            // 3. Retrieve the second number greater than 5
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var secondNumberGreaterThan5 = numbers.Where(n => n > 5).Skip(1).FirstOrDefault();
            Console.WriteLine($"3. Second number greater than 5: {secondNumberGreaterThan5}");

            #endregion

            #region LINQ - Aggregate Operators

            Console.WriteLine("LINQ - Aggregate Operators");
            Console.WriteLine("---------------------------");

            // 1. Uses Count to get the number of odd numbers in the array
            int[] oddNumbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            Console.WriteLine($"1. Number of odd numbers: {oddNumbers.Count(n => n % 2 != 0)}");

            // 2. Return a list of customers and how many orders each has
            var customerOrders = ListGenerators.CustomerList
                .Select(c => $"{c.CustomerName}: {c.Orders.Length} order(s)");
            Console.WriteLine("2. Customers and their order counts:");
            Console.WriteLine(string.Join("\n- ", customerOrders.Prepend("-")));

            // 3. Return a list of categories and how many products each has
            var categoryProducts = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => $"{g.Key}: {g.Count()} product(s)");
            Console.WriteLine("3. Categories and their product counts:");
            Console.WriteLine(string.Join("\n- ", categoryProducts.Prepend("-")));

            // 4. Get the total of the numbers in an array
            Console.WriteLine($"4. Sum of numbers: {oddNumbers.Sum()}");

            // 5-8. Operations on dictionary_english.txt
            string[] words = File.ReadAllLines("dictionary_english.txt");
            Console.WriteLine($"5. Total characters in all words: {words.Sum(w => w.Length)}");
            Console.WriteLine($"6. Length of the shortest word: {words.Min(w => w.Length)}");
            Console.WriteLine($"7. Length of the longest word: {words.Max(w => w.Length)}");
            Console.WriteLine($"8. Average word length: {words.Average(w => w.Length):F2}");

            // 9. Get the total units in stock for each product category
            var categoryStock = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => $"{g.Key}: {g.Sum(p => p.UnitsInStock)} units");
            Console.WriteLine("9. Total units in stock for each category:");
            Console.WriteLine(string.Join("\n- ", categoryStock.Prepend("-")));

            // 10. Get the cheapest price among each category's products
            var cheapestByCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => $"{g.Key}: {g.Min(p => p.UnitPrice):C}");
            Console.WriteLine("10. Cheapest price in each category:");
            Console.WriteLine(string.Join("\n- ", cheapestByCategory.Prepend("-")));

            // 11. Get the products with the cheapest price in each category (Use Let)
            var cheapestProductsByCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    g.Key,
                    CheapestProducts = g.Where(p => p.UnitPrice == g.Min(p => p.UnitPrice))
                });
            Console.WriteLine("11. Cheapest products in each category:");
            foreach (var category in cheapestProductsByCategory)
            {
                Console.WriteLine($"- {category.Key}:");
                foreach (var product in category.CheapestProducts)
                {
                    Console.WriteLine($"  * {product.ProductName} - {product.UnitPrice:C}");
                }
            }

            // 12. Get the most expensive price among each category's products
            var mostExpensiveByCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => $"{g.Key}: {g.Max(p => p.UnitPrice):C}");
            Console.WriteLine("12. Most expensive price in each category:");
            Console.WriteLine(string.Join("\n- ", mostExpensiveByCategory.Prepend("-")));

            // 13. Get the products with the most expensive price in each category
            var mostExpensiveProductsByCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    g.Key,
                    MostExpensiveProducts = g.Where(p => p.UnitPrice == g.Max(p => p.UnitPrice))
                });
            Console.WriteLine("13. Most expensive products in each category:");
            foreach (var category in mostExpensiveProductsByCategory)
            {
                Console.WriteLine($"- {category.Key}:");
                foreach (var product in category.MostExpensiveProducts)
                {
                    Console.WriteLine($"  * {product.ProductName} - {product.UnitPrice:C}");
                }
            }

            // 14. Get the average price of each category's products
            var averagePriceByCategory = ListGenerators.ProductList
                .GroupBy(p => p.Category)
                .Select(g => $"{g.Key}: {g.Average(p => p.UnitPrice):C}");
            Console.WriteLine("14. Average price in each category:");
            Console.WriteLine(string.Join("\n- ", averagePriceByCategory.Prepend("-")));


            /*
            [ Prepend ] Used to format the output in one line. 
              */


            #endregion

            #region LINQ - Ordering Operators

            Console.WriteLine("LINQ - Ordering Operators");
            Console.WriteLine("--------------------------");

            // 1. Sort a list of products by name
            var productsSortedByName = ListGenerators.ProductList.OrderBy(p => p.ProductName);
            Console.WriteLine("1. Products sorted by name:");
            Console.WriteLine(string.Join("\n- ", productsSortedByName.Take(5).Select(p => p.ProductName)) + "\n...");

            // 2. Uses a custom comparer to do a case-insensitive sort of the words in an array
            string[] wordArray = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };
            var sortedWords = wordArray.OrderBy(w => w, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("\n2. Words sorted case-insensitively:");
            Console.WriteLine(string.Join("\n- ", sortedWords));

            // 3. Sort a list of products by units in stock from highest to lowest
            var productsSortedByStock = ListGenerators.ProductList.OrderByDescending(p => p.UnitsInStock);
            Console.WriteLine("\n3. Products sorted by units in stock (highest to lowest):");
            Console.WriteLine(string.Join("\n- ", productsSortedByStock.Take(5).Select(p => $"{p.ProductName}: {p.UnitsInStock}")) + "\n...");

            // 4. Sort a list of digits, first by length of their name, and then alphabetically by the name itself
            string[] digitNames = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var sortedDigits = digitNames.OrderBy(d => d.Length).ThenBy(d => d);
            Console.WriteLine("\n4. Digits sorted by name length, then alphabetically:");
            Console.WriteLine(string.Join("\n- ", sortedDigits));

            // 5. Sort first by word length and then by a case-insensitive sort of the words in an array
            var sortedWordsByLengthThenAlpha = words.OrderBy(w => w.Length).ThenBy(w => w, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("\n5. Words sorted by length, then case-insensitively:");
            Console.WriteLine(string.Join("\n- ", sortedWordsByLengthThenAlpha));

            // 6. Sort a list of products, first by category, and then by unit price, from highest to lowest
            var productsSortedByCategoryThenPrice = ListGenerators.ProductList
                .OrderBy(p => p.Category)
                .ThenByDescending(p => p.UnitPrice);
            Console.WriteLine("\n6. Products sorted by category, then by price (highest to lowest):");
            Console.WriteLine(string.Join("\n- ", productsSortedByCategoryThenPrice.Take(10).Select(p => $"{p.Category}, {p.ProductName}: {p.UnitPrice:C}")) + "\n...");

            // 7. Sort first by word length and then by a case-insensitive descending sort of the words in an array
            var sortedWordsByLengthThenAlphaDesc = words.OrderBy(w => w.Length).ThenByDescending(w => w, StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("\n7. Words sorted by length, then case-insensitively in descending order:");
            Console.WriteLine(string.Join("\n- ", sortedWordsByLengthThenAlphaDesc));

            // 8. Create a list of all digits in the array whose second letter is 'i' that is reversed from the order in the original array
            var reversedDigitsWithI = digitNames
                .Where(d => d.Length > 1 && d[1] == 'i')
                .Reverse();
            Console.WriteLine("\n8. Reversed list of digits with 'i' as the second letter:");
            Console.WriteLine(string.Join("\n- ", reversedDigitsWithI));

            #endregion

            #region LINQ - Transformation Operators

            Console.WriteLine("LINQ - Transformation Operators");
            Console.WriteLine("--------------------------------");

            // 1. Return a sequence of just the names of a list of products
            var productNames = ListGenerators.ProductList.Select(p => p.ProductName);
            Console.WriteLine("1. Product names:");
            Console.WriteLine(string.Join("\n- ", productNames.Take(5)) + "\n...");

            // 2. Produce a sequence of the uppercase and lowercase versions of each word in the original array
            var upperLowerWords = words.Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() });
            Console.WriteLine("\n2. Words in uppercase and lowercase:");
            foreach (var pair in upperLowerWords)
            {
                Console.WriteLine($"- Upper: {pair.Upper}, Lower: {pair.Lower}");
            }

            // 3. Produce a sequence containing some properties of Products, including UnitPrice which is renamed to Price in the resulting type
            var productInfo = ListGenerators.ProductList.Select(p => new { p.ProductName, p.Category, Price = p.UnitPrice });
            Console.WriteLine("\n3. Product information with renamed Price:");
            Console.WriteLine(string.Join("\n- ", productInfo.Take(5).Select(info => $"{info.ProductName} ({info.Category}): {info.Price:C}")) + "\n...");

            // 4. Determine if the value of ints in an array match their position in the array
            int[] numberArray = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var numbersAndPositions = numberArray.Select((num, index) => new { Number = num, InPlace = (num == index) });
            Console.WriteLine("\n4. Numbers and their positions:");
            foreach (var item in numbersAndPositions)
            {
                Console.WriteLine($"- Number: {item.Number}, In Place: {item.InPlace}");
            }

            // 5. Returns all pairs of numbers from both arrays such that the number from numbersA is less than the number from numbersB
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };
            var pairs = from a in numbersA
                        from b in numbersB
                        where a < b
                        select new { A = a, B = b };
            Console.WriteLine("\n5. Pairs of numbers where A < B:");
            foreach (var pair in pairs)
            {
                Console.WriteLine($"- A: {pair.A}, B: {pair.B}");
            }

            // 6. Select all orders where the order total is less than $500
            var lowValueOrders = ListGenerators.CustomerList
                .SelectMany(c => c.Orders)
                .Where(o => o.Total < 500.00m);
            Console.WriteLine("\n6. Orders with total less than $500:");
            Console.WriteLine(string.Join("\n- ", lowValueOrders.Take(5).Select(o => $"Order ID: {o.OrderID}, Total: {o.Total:C}")) + "\n...");

            // 7. Select all orders where the order was made in 1998 or later
            var recentOrders = ListGenerators.CustomerList
                .SelectMany(c => c.Orders)
                .Where(o => o.OrderDate.Year >= 1998);
            Console.WriteLine("\n7. Orders from 1998 or later:");
            Console.WriteLine(string.Join("\n- ", recentOrders.Take(5).Select(o => $"Order ID: {o.OrderID}, Date: {o.OrderDate:d}")) + "\n...");

            #endregion

            #region LINQ - Set Operators

            Console.WriteLine("LINQ - Set Operators");
            Console.WriteLine("---------------------");

            // 1. Find the unique Category names from Product List
            var uniqueCategories = ListGenerators.ProductList.Select(p => p.Category).Distinct();
            Console.WriteLine("1. Unique product categories:");
            foreach (var category in uniqueCategories)
            {
                Console.WriteLine($"- {category}");
            }

            // 2. Produce a sequence containing the unique first letter from both product and customer names
            var productFirstLetters = ListGenerators.ProductList.Select(p => p.ProductName[0]);
            var customerFirstLetters = ListGenerators.CustomerList.Select(c => c.CustomerName[0]);
            var uniqueFirstLetters = productFirstLetters.Union(customerFirstLetters).OrderBy(c => c);
            Console.WriteLine("\n2. Unique first letters from product and customer names:");
            Console.WriteLine(string.Join(", ", uniqueFirstLetters));

            // 3. Create one sequence that contains the common first letter from both product and customer names
            var commonFirstLetters = productFirstLetters.Intersect(customerFirstLetters).OrderBy(c => c);
            Console.WriteLine("\n3. Common first letters from product and customer names:");
            Console.WriteLine(string.Join(", ", commonFirstLetters));

            // 4. Create one sequence that contains the first letters of product names that are not also first letters of customer names
            var productOnlyFirstLetters = productFirstLetters.Except(customerFirstLetters).OrderBy(c => c);
            Console.WriteLine("\n4. First letters of product names not in customer names:");
            Console.WriteLine(string.Join(", ", productOnlyFirstLetters));

            // 5. Create one sequence that contains the last three characters in each name of all customers and products, including any duplicates
            var lastThreeChars = ListGenerators.ProductList
                .Select(p => p.ProductName.Length >= 3 ? p.ProductName[^3..] : p.ProductName)
                .Concat(ListGenerators.CustomerList
                    .Select(c => c.CustomerName.Length >= 3 ? c.CustomerName[^3..] : c.CustomerName));
            Console.WriteLine("\n5. Last three characters of all customer and product names:");
            Console.WriteLine(string.Join(", ", lastThreeChars.Take(20)) + "\n...");

            /*
            String Slicing: Used the C# 8.0 { ^ } operator for more concise slicing of the last three characters.
            */


            #endregion

            #region LINQ - Partitioning Operators

            Console.WriteLine("LINQ - Partitioning Operators");
            Console.WriteLine("------------------------------");

            // 1. Get the first 3 orders from customers in Washington
            var washingtonOrders = ListGenerators.CustomerList
                .Where(c => c.Region == "WA")
                .SelectMany(c => c.Orders)
                .Take(3);
            Console.WriteLine("1. First 3 orders from customers in Washington:");
            foreach (var order in washingtonOrders)
            {
                Console.WriteLine($"- Order ID: {order.OrderID}, Date: {order.OrderDate:d}, Total: {order.Total:C}");
            }
            Console.WriteLine();

            // 2. Get all but the first 2 orders from customers in Washington
            var washingtonOrdersSkipTwo = ListGenerators.CustomerList
                .Where(c => c.Region == "WA")
                .SelectMany(c => c.Orders)
                .Skip(2);
            Console.WriteLine("2. All but first 2 orders from customers in Washington:");
            foreach (var order in washingtonOrdersSkipTwo.Take(5)) // Display first 5 for brevity
            {
                Console.WriteLine($"- Order ID: {order.OrderID}, Date: {order.OrderDate:d}, Total: {order.Total:C}");
            }
            Console.WriteLine("...");
            Console.WriteLine();

            // 3. Return elements starting from the beginning of the array until a number is hit that is less than its position in the array
            int[] numberSequence = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var elementsUntilLessThanPosition = numberSequence.TakeWhile((n, index) => n >= index);
            Console.WriteLine("3. Elements until less than position:");
            Console.WriteLine(string.Join(", ", elementsUntilLessThanPosition));
            Console.WriteLine();

            // 4. Get the elements of the array starting from the first element divisible by 3
            var elementsStartingFromDivisibleByThree = numberSequence.SkipWhile(n => n % 3 != 0);
            Console.WriteLine("4. Elements starting from first divisible by 3:");
            Console.WriteLine(string.Join(", ", elementsStartingFromDivisibleByThree));
            Console.WriteLine();

            // 5. Get the elements of the array starting from the first element less than its position
            var elementsStartingFromLessThanPosition = numberSequence.SkipWhile((n, index) => n >= index);
            Console.WriteLine("5. Elements starting from first less than position:");
            Console.WriteLine(string.Join(", ", elementsStartingFromLessThanPosition));
            Console.WriteLine();


            /* Skip: Ignore the first X elements.
               Take: Select the first X elements.
               TakeWhile: Select elements as long as a condition is true.
               SkipWhile: Ignore elements as long as a condition is true, then select the rest.*/

            #endregion

            #region LINQ - Quantifiers

            Console.WriteLine("LINQ - Quantifiers");
            Console.WriteLine("-------------------");

            // 1. Determine if any of the words in dictionary_english.txt contain the substring 'ei'.
            string[] dictionaryWords = File.ReadAllLines("dictionary_english.txt");
            bool anyWordsWithEi = dictionaryWords.Any(w => w.Contains("ei"));
            Console.WriteLine($"1. Any words contain 'ei': {anyWordsWithEi}");
            Console.WriteLine();

            // 2. Return a grouped list of products only for categories that have at least one product that is out of stock.
            var categoriesWithOutOfStock =
                from product in ListGenerators.ProductList
                group product by product.Category into categoryGroup
                where categoryGroup.Any(p => p.UnitsInStock == 0)
                select new { Category = categoryGroup.Key, Products = categoryGroup.ToList() };

            Console.WriteLine("2. Categories with at least one out-of-stock product:");
            foreach (var category in categoriesWithOutOfStock)
            {
                Console.WriteLine($"- {category.Category}:");
                foreach (var product in category.Products.Take(3)) // Display first 3 products for brevity
                {
                    Console.WriteLine($"  * {product.ProductName} (In stock: {product.UnitsInStock})");
                }
                if (category.Products.Count > 3)
                {
                    Console.WriteLine("  ...");
                }
            }
            Console.WriteLine();

            // 3. Return a grouped list of products only for categories that have all of their products in stock.
            var categoriesAllInStock =
                from product in ListGenerators.ProductList
                group product by product.Category into categoryGroup
                where categoryGroup.All(p => p.UnitsInStock > 0)
                select new { Category = categoryGroup.Key, Products = categoryGroup.ToList() };

            Console.WriteLine("3. Categories with all products in stock:");
            foreach (var category in categoriesAllInStock)
            {
                Console.WriteLine($"- {category.Category}:");
                foreach (var product in category.Products.Take(3)) // Display first 3 products for brevity
                {
                    Console.WriteLine($"  * {product.ProductName} (In stock: {product.UnitsInStock})");
                }
                if (category.Products.Count > 3)
                {
                    Console.WriteLine("  ...");
                }
            }
            Console.WriteLine();

            #endregion

            #region LINQ - Grouping Operators

            Console.WriteLine("LINQ - Grouping Operators");
            Console.WriteLine("--------------------------");

            // 1. Use group by to partition a list of numbers by their remainder when divided by 5.
            List<int> numberList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var groupedByRemainder = numberList.GroupBy(n => n % 5);

            Console.WriteLine("1. Numbers grouped by remainder when divided by 5:");
            foreach (var group in groupedByRemainder)
            {
                Console.WriteLine($"Remainder {group.Key}: {string.Join(", ", group)}");
            }
            Console.WriteLine();

            // 2. Use group by to partition a list of words by their first letter.
            var wordsByFirstLetter = dictionaryWords.GroupBy(w => w[0]);
            Console.WriteLine("2. Words grouped by first letter:");
            foreach (var group in wordsByFirstLetter.Take(5)) // Display first 5 groups for brevity
            {
                Console.WriteLine($"'{group.Key}': {group.Count()} words");
            }
            Console.WriteLine("...");
            Console.WriteLine();

            // 3. Use GroupBy with a custom comparer that matches words that consist of the same characters.
            string[] wordCollection = { "from", "salt", "earn", "last", "near", "form" };
            var groupedBySameCharacters = wordCollection.GroupBy(word => new string(word.OrderBy(c => c).ToArray()));

            Console.WriteLine("3. Words grouped by same characters:");
            foreach (var group in groupedBySameCharacters)
            {
                Console.WriteLine($"Group: {string.Join(", ", group)}");
            }
            Console.WriteLine();

            #endregion

        }
    }
}