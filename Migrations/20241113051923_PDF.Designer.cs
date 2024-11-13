﻿// <auto-generated />
using System;
using Inventario_Hotel.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventarioHotel.Migrations
{
    [DbContext(typeof(AplicationDbContext.InventoryContext))]
    [Migration("20241113051923_PDF")]
    partial class PDF
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Inventario_Hotel.Entities.Almacenista", b =>
                {
                    b.Property<int>("PkAlmacenista")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("contrasena")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("PkAlmacenista");

                    b.ToTable("Almacenistas");
                });

            modelBuilder.Entity("Inventario_Hotel.Entities.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Codigo")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaEntrada")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaSalida")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MaxPiezas")
                        .HasColumnType("int");

                    b.Property<int>("MinPiezas")
                        .HasColumnType("int");

                    b.Property<int>("Pieza")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
