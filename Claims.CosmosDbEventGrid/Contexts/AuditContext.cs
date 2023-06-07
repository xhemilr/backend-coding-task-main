﻿using Claims.CosmosDbEventGrid.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.CosmosDbEventGrid.Contexts
{
    public class AuditContext : DbContext
    {
        public AuditContext(DbContextOptions<AuditContext> options) : base(options)
        {
        }

        public DbSet<ClaimAudit> ClaimAudits { get; set; }

        public DbSet<CoverAudit> CoverAudits { get; set; }
    }
}
