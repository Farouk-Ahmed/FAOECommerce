public static class ApiRoutes
{
    public static class Category
    {
        public const string Base = "api/category";
        public const string GetAll = Base;
        public const string GetById = Base + "/{id}";
        public const string Create = Base;
        public const string Update = Base + "/{id}";
        public const string Delete = Base + "/{id}";
    }
    public static class Product
    {
        public const string Base = "api/product";
        public const string GetAll = Base;
        public const string Get = Base + "/{id}";
        public const string Create = Base;
        public const string Update = Base + "/{id}";
        public const string Delete = Base + "/{id}";
        public const string GetByCategory = Base + "/by-category/{categoryId}";
    }
    public static class ShoppingCart
    {
        public const string Base = "api/shoppingcart";
        public const string GetCart = Base + "/user/{userId}";
        public const string AddToCart = Base;
        public const string UpdateCartItem = Base + "/{id}";
        public const string RemoveFromCart = Base + "/{id}";
        public const string Checkout = Base + "/checkout/{userId}";
    }
    public static class Order
    {
        public const string Base = "api/order";
        public const string GetAll = Base;
        public const string GetByID = Base + "/{id}";
        public const string GetByUser = Base + "/by-user/{userId}";
        public const string Create = Base;
        public const string Update = Base + "/{id}";
        public const string Delete = Base + "/{id}";
    }
}
