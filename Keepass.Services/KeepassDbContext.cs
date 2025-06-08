using KeePassLib;
using KeePassLib.Keys;
using KeePassLib.Serialization;

namespace Keepass.Services;

public class KeepassDbContext
{
    private void LoadKeePassEntries(string filePath, string password)
    {
        try
        {
            // Load the database
            var compKey = new CompositeKey();
            compKey.AddUserKey(new KcpPassword(password));

            var ioConn = new IOConnectionInfo { Path = filePath };

            var db = new PwDatabase();
            db.Open(ioConn, compKey, null);

            // Traverse the tree and list all entries
            var entries = db.RootGroup.GetEntries(true); // true = recursive

            if (!entries.Any())
            {

                return;
            }

            var sb = new System.Text.StringBuilder();
            foreach (var entry in entries)
            {
                string title = entry.Strings.ReadSafe("Title");
                string user = entry.Strings.ReadSafe("UserName");
                string pass = entry.Strings.ReadSafe("Password");
                sb.AppendLine($"Title: {title}");
                sb.AppendLine($"Username: {user}");
                sb.AppendLine($"Password: {pass}");
                sb.AppendLine("----------------------");
            }

        }
        catch (Exception ex)
        {
            //
        }
    }

}