namespace Inventory.Model
{
    class Item
    {
        public int Id { get; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public Item(string name, int quantity, int price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }


    }
}
