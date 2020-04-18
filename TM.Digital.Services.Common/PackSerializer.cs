using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Digital.Model;
using TM.Digital.Model.Cards;
using TM.Digital.Model.Corporations;

namespace TM.Digital.Services.Common
{
    public class PackSerializer
    {
        public static async Task<CardReferencesHolder> GtPacks(string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            CardReferencesHolder all;
            var files = di.GetFiles("*_packs.json").OrderBy(fi => fi.CreationTime).Reverse();
            var TheOne = files.FirstOrDefault();

            if (TheOne != null && File.Exists(TheOne.FullName))
            {
                all = System.Text.Json.JsonSerializer.Deserialize<CardReferencesHolder>(await File.ReadAllTextAsync(TheOne.FullName));
            }
            else
            {
                all = new CardReferencesHolder()
                {
                    Packs = new List<ExtensionPack>()
                    {
                        new ExtensionPack()
                        {
                            Name = Extensions.Base,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()
                        },
                        new ExtensionPack()
                        {
                            Name = Extensions.Prelude,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()
                        },
                        new ExtensionPack()
                        {
                            Name = Extensions.Colonies,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()
                        },
                        new ExtensionPack()
                        {
                            Name = Extensions.Turnmoil,
                            Corporations = new List<Corporation>(),
                            Patents = new List<Patent>(),
                            Preludes = new List<Prelude>()
                        }
                    }
                };
            }
            return all;
        }
    }
}
