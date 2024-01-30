

using bmstu_bot.Bot;

namespace bmstu_bot.Types
{
    internal class Complain
    {
        public Models.Complain compalin { get; set; }


        public async Task<int> Add()
        {
            using (Models.BmstuBotContext db = new Models.BmstuBotContext())
            {
                try
                {
                    var addedItem = await db.Complains.AddAsync(this.compalin);

                    await db.SaveChangesAsync();
                    return addedItem.Entity.Id;
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Exeption in Complain.cs\nFunction: Complain.Add()\n\n");
                }
            }
            return 0;
        }

        public Task Update()
        {

            return Task.Run(async () =>
            {
                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        var cmpl = db.Complains.Where(compl => compl.Id == this.compalin.Id).FirstOrDefault();
                        db.Update(cmpl).CurrentValues.SetValues(this.compalin);

                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in Complain.cs\nFunction: Complain.Update()\\n\n");
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
                        var cmpl = db.Complains.Where(cmpl => cmpl.Id == this.compalin.Id).FirstOrDefault();
                        if (cmpl is not null)
                        {
                            this.compalin = cmpl;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No such user exeption in Complain.cs\nFunction: Complain.Get()\n\n {ex}\n\n");
                    }

                }
            });

        }

        public Task Send(TelegramBotClient bot, string generatedMessage)
        {
            return Task.Run(async () =>
            {
                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        var allAdmins = db.Admins;

                        await allAdmins.ForEachAsync(async admin =>
                        {
                            var msg = await bot.SendTextMessageAsync(admin.ChatId, generatedMessage, parseMode: ParseMode.Markdown);
                            var entry = new Types.Entry { entry = new Models.Entry() { AdminChat = admin.ChatId, ComplainId = this.compalin.Id, MessageId = msg.MessageId } };
                            await entry.Add();
                        });
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in Send.cs\nFunction: Complain.Send()\\n\n");
                    }

                }
            });
        }

        public Task<List<Complain>> GetChat()
        {
            return Task.Run(async () =>
            {
                List<Complain> complains = new List<Complain>();
                int? complain_id = this.compalin.Prev;

                while (complain_id is not null)
                {

                    using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                    {
                        try
                        {
                            Complain sub_complain = new Complain() { compalin = new Models.Complain() { Id = (int)complain_id } };
                            await sub_complain.Get();
                            complains.Add(sub_complain);

                            complain_id = sub_complain.compalin.Prev;

                        }

                        catch (DbUpdateException ex)
                        {
                            Console.WriteLine($"Exeption in Send.cs\nFunction: Complain.GetChat()\\n\n");
                        }

                    }
                }

                complains.Reverse();
                return complains;
            });
           
        }

    }
}
