
namespace bmstu_bot.Types
{
    internal class User
    {
        public Models.User user { get; set; }

        public Task Add()
        {

            return Task.Run(async () =>
            {

                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                
                        await db.Users.AddAsync(this.user);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in User.cs\nFunction: User.Add()\n\n {ex}");
                    }
                }
            });
        }

        public Task Update()
        {
            return Task.Run(async () =>
            {
                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        var user = db.Users.Where(usr => usr.ChatId == this.user.ChatId).First();
                        db.Update(user).CurrentValues.SetValues(this.user);

                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in User.cs\nFunction: User.Update()\\n\n");
                    }
                }
            });
        }

        public Task Get()
        {
            return Task.Run(() =>
            {
                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        Console.WriteLine(this.user.ChatId);
                        var usr = db.Users.Where(usr => usr.ChatId == this.user.ChatId).FirstOrDefault();
                        if (usr is not null)
                        {
                            user = usr;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No such user exeption in User.cs\nFunction: User.Get()\n\n {ex}\n\n");
                    }

                }
            });

        }


    }
}
