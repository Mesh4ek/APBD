using Task5.Api.Models;

namespace Task5.Api.Data;

public static class AnimalsRepository
{
    public static readonly List<Animal> Animals = new()
    {
        new Animal { Id = 1, Name = "Buddy", Category = "Dog", Weight = 25.5, FurColor = "Brown" },
        new Animal { Id = 2, Name = "Whiskers", Category = "Cat", Weight = 10.0, FurColor = "Black" },
        new Animal { Id = 3, Name = "Chirpy", Category = "Bird", Weight = 0.3, FurColor = "Yellow" },
        new Animal { Id = 4, Name = "Nibbles", Category = "Hamster", Weight = 0.5, FurColor = "White" },
        new Animal { Id = 5, Name = "Max", Category = "Dog", Weight = 30.0, FurColor = "Golden" },
        new Animal { Id = 6, Name = "Luna", Category = "Cat", Weight = 8.0, FurColor = "Gray" },
        new Animal { Id = 7, Name = "Rex", Category = "Dog", Weight = 40.0, FurColor = "Black" },
        new Animal { Id = 8, Name = "Snowball", Category = "Rabbit", Weight = 2.3, FurColor = "White" },
        new Animal { Id = 9, Name = "Goldie", Category = "Fish", Weight = 0.1, FurColor = "Orange" }
    };
}