using System;
using ASSLINQ;
using static LINQ_Demo02.ListGenerators;

namespace LINQ_Demo02
{
    internal class Program
    {
        static void Main()
        {

            #region Filtration Operator - [ Where & OfType ] 
            /*
                Where: Filters a sequence based on a predicate.
                OfType: Filters elements of a specific type from a sequence.
            */

            // Where operator
            var expensiveProducts = ProductList.Where(p => p.UnitPrice > 50);
            Console.WriteLine("Expensive products (price > 50):");
            foreach (var product in expensiveProducts)
            {
                Console.WriteLine($"{product.ProductName}: {product.UnitPrice:C}");
            }
            // Ensure the predicate in Where doesn't throw exceptions.


            // OfType operator
            var stringList = new List<object> { "apple", 1, "banana", 2, "cherry" };
            var onlyStrings = stringList.OfType<string>();
            Console.WriteLine("\nOnly strings from mixed list:");
            foreach (var item in onlyStrings)
            {
                Console.WriteLine(item);
            }

            // OfType won't throw errors, but may return an empty sequence if no matching types are found.

            #endregion

            #region Transformation Operators [ Select - SelectMany ]
            /*
                Select: Projects each element of a sequence into a new form.
                SelectMany: Flattens a sequence of sequences into a single sequence.
            */
            // Select operator
            var productNames = ProductList.Select(p => p.ProductName);
            Console.WriteLine("\nProduct names:");
            foreach (var name in productNames)
            {
                Console.WriteLine(name);
            }

            // Ensure the selector function doesn't throw exceptions.



            // SelectMany operator
            var allOrders = CustomerList.SelectMany(c => c.Orders);
            Console.WriteLine("\nAll orders:");
            foreach (var order in allOrders.Take(5)) // Showing first 5 for brevity
            {
                Console.WriteLine($"Order ID: {order.OrderID}, Total: {order.Total:C}");
            }

            // For SelectMany, make sure the inner sequences are not null.


            #endregion

            #region Ordering Operators
            //OrderBy/ThenBy: Sorts elements in ascending order.
            //OrderByDescending / ThenByDescending: Sorts elements in descending order.

            // OrderBy
            var orderedProducts = ProductList.OrderBy(p => p.UnitPrice);
            Console.WriteLine("\nProducts ordered by price (ascending):");
            foreach (var product in orderedProducts.Take(5))
            {
                Console.WriteLine($"{product.ProductName}: {product.UnitPrice:C}");
            }

            // OrderByDescending
            var orderedProductsDesc = ProductList.OrderByDescending(p => p.UnitPrice);
            Console.WriteLine("\nProducts ordered by price (descending):");
            foreach (var product in orderedProductsDesc.Take(5))
            {
                Console.WriteLine($"{product.ProductName}: {product.UnitPrice:C}");
            }

            // ThenBy
            var orderedCustomers = CustomerList.OrderBy(c => c.Country).ThenBy(c => c.CustomerName);
            Console.WriteLine("\nCustomers ordered by country, then by name:");
            foreach (var customer in orderedCustomers.Take(5))
            {
                Console.WriteLine($"{customer.Country}: {customer.CustomerName}");
            }


            //Ensure the key selector functions don't throw exceptions.
            //Be cautious with null values in the key selector.

            #endregion

            #region Element Operators [ Immidiate Execution ]
            // These operators return a single element from a sequence.

            // First
            var firstExpensiveProduct = ProductList.First(p => p.UnitPrice > 100);
            Console.WriteLine($"\nFirst product with price > 100: {firstExpensiveProduct.ProductName}");

            // FirstOrDefault
            var firstCheapProduct = ProductList.FirstOrDefault(p => p.UnitPrice < 1);
            Console.WriteLine(firstCheapProduct != null
                ? $"First product with price < 1: {firstCheapProduct.ProductName}"
                : "No product with price < 1");

            // Single
            try
            {
                var singleProduct = ProductList.Single(p => p.ProductID == 1);
                Console.WriteLine($"\nSingle product with ID 1: {singleProduct.ProductName}");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No single product found with the specified condition");
            }

            // ElementAt
            var thirdProduct = ProductList.ElementAt(2);
            Console.WriteLine($"\nThird product: {thirdProduct.ProductName}");

            // Use FirstOrDefault, LastOrDefault, SingleOrDefault when the sequence might be empty.
            //Ensure predicates don't throw exceptions.


            #endregion

            #region Aggregate Operators [ Immidiate Execution ]
            // These operators perform calculations on a sequence.

            // Count
            int productCount = ProductList.Count();
            Console.WriteLine($"\nTotal number of products: {productCount}");

            // Sum
            decimal totalInventoryValue = ProductList.Sum(p => p.UnitPrice * p.UnitsInStock);
            Console.WriteLine($"Total inventory value: {totalInventoryValue:C}");

            // Average
            double averagePrice = ProductList.Average(p => (double)p.UnitPrice);
            Console.WriteLine($"Average product price: {averagePrice:C}");

            // Min and Max
            decimal lowestPrice = ProductList.Min(p => p.UnitPrice);
            decimal highestPrice = ProductList.Max(p => p.UnitPrice);
            Console.WriteLine($"Price range: {lowestPrice:C} - {highestPrice:C}");

            // Aggregate
            string categoryList = ProductList.Select(p => p.Category).Distinct()
                                             .Aggregate((current, next) => current + ", " + next);
            Console.WriteLine($"All categories: {categoryList}");


            //Be cautious with empty sequences (except for Count).
            //Handle potential overflow in Sum and Average.

            #endregion

            #region Casting Operators [ Immidiate Execution ]
            // These operators convert LINQ query results to various collection types.

            // ToList
            List<string> productNameList = ProductList.Select(p => p.ProductName).ToList();
            Console.WriteLine($"\nFirst 5 product names: {string.Join(", ", productNameList.Take(5))}");

            // ToArray
            string[] categoryArray = ProductList.Select(p => p.Category).Distinct().ToArray();
            Console.WriteLine($"Categories as array: {string.Join(", ", categoryArray)}");

            // ToDictionary
            var productDictionary = ProductList.ToDictionary(p => p.ProductID, p => p.ProductName);
            Console.WriteLine($"Product with ID 1: {productDictionary[1]}");

            // Cast
            IEnumerable<object> mixedList = new List<object> { 1, "two", 3, "four", 5 };
            try
            {
                IEnumerable<int> intList = mixedList.Cast<int>();
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Cast failed: not all elements are integers");
            }

            // For ToDictionary, ensure the key selector produces unique keys.


            #endregion

            #region Generation Operators
            // These operators create new sequences.

            // Range
            var numberSequence = Enumerable.Range(1, 5);
            Console.WriteLine($"\nNumber sequence: {string.Join(", ", numberSequence)}");

            // Repeat
            var repeatedValue = Enumerable.Repeat("Hello", 3);
            Console.WriteLine($"Repeated value: {string.Join(", ", repeatedValue)}");

            // Empty
            var emptyCollection = Enumerable.Empty<int>();
            Console.WriteLine($"Empty collection count: {emptyCollection.Count()}");

            // Be cautious with large ranges to avoid excessive memory usage.



            #endregion

            #region Set Operators
            // These operators perform set operations on sequences.
            // Consider using appropriate equality comparers for complex types.


            var category1 = new[] { "Beverages", "Condiments", "Confections" };
            var category2 = new[] { "Confections", "Dairy Products", "Beverages" };

            // Distinct
            var distinctCategories = ProductList.Select(p => p.Category).Distinct();
            Console.WriteLine($"\nDistinct categories: {string.Join(", ", distinctCategories)}");

            // Union
            var unionCategories = category1.Union(category2);
            Console.WriteLine($"Union of categories: {string.Join(", ", unionCategories)}");

            // Intersect
            var intersectCategories = category1.Intersect(category2);
            Console.WriteLine($"Intersection of categories: {string.Join(", ", intersectCategories)}");

            // Except
            var exceptCategories = category1.Except(category2);
            Console.WriteLine($"Categories in 1 but not in 2: {string.Join(", ", exceptCategories)}");
            #endregion

            #region Quantifier Operators -  Return Boolean
            // These operators check conditions across a sequence.

            // Any
            bool hasExpensiveProducts = ProductList.Any(p => p.UnitPrice > 100);
            Console.WriteLine($"\nAny product more expensive than $100: {hasExpensiveProducts}");
            // Be aware that Any and All behave differently on empty sequences.

            // All
            bool allInStock = ProductList.All(p => p.UnitsInStock > 0);
            Console.WriteLine($"All products in stock: {allInStock}");
            // Be aware that Any and All behave differently on empty sequences.

            // Contains
            var searchProduct = ProductList.First();
            bool containsProduct = ProductList.Contains(searchProduct);
            Console.WriteLine($"ProductList contains the first product: {containsProduct}");
            #endregion

            #region Zip Operator
            // Combines two sequences into one based on a specified function.
            // Ensure both sequences have elements. Zip stops when the shorter sequence ends.

            var productsNames = ProductList.Take(5).Select(p => p.ProductName);
            var productPrices = ProductList.Take(5).Select(p => p.UnitPrice);
            var zippedProducts = productNames.Zip(productPrices, (name, price) => $"{name}: {price:C}");
            Console.WriteLine("\nZipped products and prices:");
            foreach (var item in zippedProducts)
            {
                Console.WriteLine(item);
            }
            #endregion

            #region Grouping Operators
            // Groups elements based on a specified key.

            // GroupBy
            var groupedProducts = ProductList.GroupBy(p => p.Category);
            Console.WriteLine("\nProducts grouped by category:");
            foreach (var group in groupedProducts)
            {
                Console.WriteLine($"{group.Key}: {group.Count()} products");
            }
            // Ensure the key selector doesn't return null.
            //Be cautious with memory usage for large datasets
            #endregion

            #region Partitioning Operators
            // These operators split a sequence into parts.
            // Check sequence length before using Skip to avoid empty results.

            // Take
            var first5Products = ProductList.Take(5);
            Console.WriteLine("\nFirst 5 products:");
            foreach (var product in first5Products)
            {
                Console.WriteLine(product.ProductName);
            }

            // Skip
            var productsAfterFirst5 = ProductList.Skip(5).Take(5);
            Console.WriteLine("\nProducts 6-10:");
            foreach (var product in productsAfterFirst5)
            {
                Console.WriteLine(product.ProductName);
            }

            // TakeWhile
            var takeWhileUnder20 = ProductList.TakeWhile(p => p.UnitPrice < 20);
            Console.WriteLine("\nProducts taken while price is under $20:");
            foreach (var product in takeWhileUnder20)
            {
                Console.WriteLine($"{product.ProductName}: {product.UnitPrice:C}");
            }
            #endregion

            #region Let and Into
            // 'let' introduces new variables in a query,
            // 'into' continues a query after a group or select clause.


            var categorySummary = from p in ProductList
                                  group p by p.Category into categoryGroup
                                  let averagePriceInCategory = categoryGroup.Average(p => p.UnitPrice)
                                  select new
                                  {
                                      Category = categoryGroup.Key,
                                      ProductCount = categoryGroup.Count(),
                                      AveragePrice = averagePriceInCategory
                                  };

            Console.WriteLine("\nCategory summary:");
            foreach (var summary in categorySummary)
            {
                Console.WriteLine($"{summary.Category}: {summary.ProductCount} products, Avg price: {summary.AveragePrice:C}");
            }
            // Ensure variables introduced with 'let' are used within the same query.
            //Be cautious with complex computations in 'let' clauses as they may impact performance.

            #endregion

        }
    }
}
