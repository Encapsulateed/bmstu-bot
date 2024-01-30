
namespace bmstu_bot.Types
{
    internal class Entry
    {
        public Models.Entry entry { get; set; }
        public Task Add()
        {

            return Task.Run(async () =>
            {

                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        await db.Entries.AddAsync(this.entry);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in Entry.cs\nFunction: Entry.Add()\n\n");
                    }
                }
            });
        }

        // Записей с одним Id ровно столько, сколько админов в проекте 
        public static List<Entry> Get(int compainId)
        {

            using (Models.BmstuBotContext db = new Models.BmstuBotContext())
            {
                try
                {
                    var entries = db.Entries.Where(entry => entry.ComplainId == compainId);
                    var outLst = new List<Entry>();

                    foreach (var entry in entries)
                    {
                        outLst.Add(new Entry() { entry = entry });
                    }

                    //Отрпавили удалили, не задерживаемся.
                    Task.Run(() =>
                   {
                       db.Entries.RemoveRange(entries);
                   });


                    return outLst;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No enties user exeption in Entry.cs\nFunction: Entry.Get()\n\n {ex}\n\n");
                }

            }

            return null;
        }

        public static Entry GetEntryByMessageIdAndAdmin(long adminChat, long messageId)
        {
            using (Models.BmstuBotContext db = new Models.BmstuBotContext())
            {
                try
                {
                    var entr = db.Entries.Where(entry => (entry.MessageId == messageId && entry.AdminChat == adminChat)).FirstOrDefault();

                    return new Entry() { entry = entr };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No enties user exeption in User.cs\nFunction: Entry.GetEntryByMessageIdAndAdmin()\n\n {ex}\n\n");
                }
            }
            return null;
        }

    }
}
