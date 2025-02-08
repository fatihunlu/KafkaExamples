using KafkaDemo.Consumer.Models;
using MongoDB.Driver;

namespace KafkaDemo.Consumer;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(IMongoClient client)
    {
        var database = client.GetDatabase("socialMedia");
        _usersCollection = database.GetCollection<User>("users");
    }

    public void AddFollower(int userId, int followerId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
        var update = Builders<User>.Update.Push(u => u.Followers, new Follower {
            FollowerId = followerId,
            FollowedAt = DateTime.UtcNow
        });

        var options = new UpdateOptions { IsUpsert = true };

        var result = _usersCollection.UpdateOne(filter, update, options);

        if (result.UpsertedId != null)
        {
            Console.WriteLine($"A new user was created with the ID: {result.UpsertedId}");
        }
        else if (result.ModifiedCount == 0)
        {
            Console.WriteLine("No user was updated. Check if the user ID is correct.");
        }
        else
        {
            Console.WriteLine("User updated successfully.");
        }
    }
}