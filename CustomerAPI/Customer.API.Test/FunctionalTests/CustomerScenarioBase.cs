namespace Customer.API.Test.FunctionalTests
{
    public class CustomerScenarioBase
    {
        public static string CustomersUrlBase => "api/v1/customers";

        public static class Get
        {
            public static string Customers(string name = null)
                => !string.IsNullOrEmpty(name)
                    ? $"{CustomersUrlBase}?name={name}"
                    : $"{CustomersUrlBase}";

            public static string CustomerById(int id)
                => $"{CustomersUrlBase}/{id}";

        }

        public static class Post
        {
            public static string CreateCustomer
                => CustomersUrlBase;
        }

        public static class Delete
        {
            public static string CustomerById(int id)
                => $"{CustomersUrlBase}/{id}";
        }

        public static class Put
        {
            public static string CustomerById(int id)
                => $"{CustomersUrlBase}/{id}";
        }
    }
}
