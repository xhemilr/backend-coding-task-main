﻿namespace Claims.Infrastructure.Configurations
{
    public class CosmosDbConfig
    {
        public string Account { get; set; }

        public string Key { get; set; }

        public string DatabaseName { get; set; }

        public List<ContainerInfo> Containers { get; set; }
    }

    public class ContainerInfo
    {
        public string Name { get; set; }

        public string PartitionKey { get; set; }
    }
}
