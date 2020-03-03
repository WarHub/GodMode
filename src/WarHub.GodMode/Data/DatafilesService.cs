using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.Workspaces.BattleScribe;

namespace WarHub.GodMode.Data
{
    public class DatafilesService
    {
        private readonly string dataDir;

        public DatafilesService(IConfiguration configuration)
        {
            dataDir = configuration["DataDir"];
        }

        public IEnumerable<string> GetDatafiles()
        {
            return Directory.EnumerateFiles(dataDir, "*", SearchOption.AllDirectories)
                .Where(x => x.EndsWith(".cat", StringComparison.OrdinalIgnoreCase) || x.EndsWith(".gst", StringComparison.OrdinalIgnoreCase));
        }

        public IDatafileInfo<SourceNode> GetDatafileInfo(string filename)
        {
            var filepath = Path.Combine(dataDir, filename);
            return filepath.LoadSourceFileAuto();
        }
    }
}
