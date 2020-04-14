﻿using Microsoft.Extensions.Configuration;
using WarHub.ArmouryModel.ProjectModel;
using WarHub.ArmouryModel.Workspaces.BattleScribe;

namespace WarHub.GodMode.Data
{
    public class DatafilesService
    {
        private readonly string dataDir;
        private readonly IWorkspace workspace;

        public DatafilesService(IConfiguration configuration)
        {
            dataDir = configuration["DataDir"];
            workspace = XmlWorkspace.CreateFromDirectory(dataDir);
        }

        public IWorkspace GetLocalFsWorkspace() => workspace;
    }
}
