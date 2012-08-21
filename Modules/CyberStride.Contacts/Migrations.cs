using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Indexing;
using CyberStride.Contacts.Models;

namespace CyberStride.Contacts.DataMigrations {
    public class Migrations : DataMigrationImpl {
        
        public int Create() {
			// Creating table ContactPartRecord
			SchemaBuilder.CreateTable("ContactPartRecord", table => table
				.ContentPartRecord()
				.Column("Name", DbType.String)
				.Column("Phone", DbType.String)
				.Column("Email", DbType.String)
				.Column("Company", DbType.String)
				.Column("CurrentWebsite", DbType.String)
				.Column("Topic", DbType.String)
				.Column("Message", DbType.String)
                .Column("ContactDateUtc", DbType.DateTime)
			);
            
            SchemaBuilder.CreateTable("ContactFormPartRecord", table => table
                .ContentPartRecord()
                .Column("EmailTo", DbType.String));

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ContactFormPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("ContactFormWidget", cfg => cfg
                .WithPart("ContactFormPart")
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));
        
            ContentDefinitionManager.AlterTypeDefinition("Contact Page", cfg => cfg
                .WithPart("RoutePart")
                .WithPart("BodyPart")
                .WithPart("LocalizationPart")
                .WithPart("ContactFormPart")
                .Creatable()
                .Indexed()
                );
            
            ContentDefinitionManager.AlterTypeDefinition("Contact",
               cfg => cfg
                   .WithPart("ContactPart")
                   .WithPart("CommonPart")
                );
 
            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("Contact Page", cfg=>
                cfg.WithPart("CommonPart"));
            return 2;
        }
    }
}