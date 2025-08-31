using Microsoft.EntityFrameworkCore;
using WeddingGiftList.Api.Models;

namespace WeddingGiftList.Api.Data;

public class WeddingGiftListContext(DbContextOptions<WeddingGiftListContext> options) : DbContext(options)
{
    public DbSet<Gift> Gifts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Gift entity
        modelBuilder.Entity<Gift>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.TakenByGuestName).HasMaxLength(100);
            entity.Property(e => e.Version).IsConcurrencyToken();
        });

        // Seed data
        modelBuilder.Entity<Gift>().HasData(
            new Gift
            {
                Id = 1,
                Name = "Porta-facas",
                Description = "Mantém as facas organizadas, seguras e sempre à mão, trazendo praticidade e estilo para a cozinha.",

            },
            new Gift
            {
                Id = 2,
                Name = "Organizadores de armário",
                Description = "Conjunto de organizadores para manter o armário sempre arrumado",

            },
            new Gift
            {
                Id = 3,
                Name = "Aparelho de jantar porcelana branco",
                Description = "Conjunto completo de pratos em porcelana branca elegante",

            },
            new Gift
            {
                Id = 4,
                Name = "Conjunto de travessas brancas",
                Description = "Travessas em porcelana branca para servir com elegância",

            },
            new Gift
            {
                Id = 5,
                Name = "Conjunto de pratos de porcelana sobremesa",
                Description = "Pratos pequenos de porcelana para sobremesas e aperitivos",

            },
            new Gift
            {
                Id = 6,
                Name = "Talheres de sobremesa",
                Description = "Conjunto de talheres especiais para sobremesas",

            },
            new Gift
            {
                Id = 7,
                Name = "Tapetes de banheiro",
                Description = "Tapetes macios e absorventes para o banheiro",

            },
            new Gift
            {
                Id = 8,
                Name = "Tábua de carne",
                Description = "Tábua de madeira resistente para corte de carnes",

            },
            new Gift
            {
                Id = 9,
                Name = "Toalha de mesa 6 lugares/ cores neutras",
                Description = "Toalha de mesa elegante para 6 pessoas em cores neutras",

            },
            new Gift
            {
                Id = 10,
                Name = "Jogo de cama casal queen 400 fios/ cores neutras",
                Description = "Jogo de cama queen de alta qualidade, 400 fios, cores neutras",

            },
            new Gift
            {
                Id = 11,
                Name = "Cortinas",
                Description = "Cortinas elegantes para decorar e controlar a luminosidade",

            },
            new Gift
            {
                Id = 12,
                Name = "Conjunto de talheres de servir inox",
                Description = "Talheres em inox para servir pratos principais",

            },
            new Gift
            {
                Id = 13,
                Name = "Lixeiras para banheiro inox",
                Description = "Lixeiras em aço inoxidável para o banheiro",

            },
            new Gift
            {
                Id = 14,
                Name = "Varal de chão",
                Description = "Varal prático e resistente para secar roupas",

            },
            new Gift
            {
                Id = 15,
                Name = "Jarra de cristal",
                Description = "Jarra elegante em cristal para servir bebidas",

            },
            new Gift
            {
                Id = 16,
                Name = "Saleiro cerâmica",
                Description = "Saleiro em cerâmica para temperos",

            },
            new Gift
            {
                Id = 17,
                Name = "Açucareiro cerâmica",
                Description = "Açucareiro em cerâmica para acompanhar o café",

            },
            new Gift
            {
                Id = 18,
                Name = "Pipoqueira",
                Description = "Pipoqueira prática para fazer pipoca caseira",

            },
            new Gift
            {
                Id = 19,
                Name = "Frigideira funda em cerâmica antiaderente",
                Description = "Frigideira funda com revestimento cerâmico antiaderente",

            },
            new Gift
            {
                Id = 20,
                Name = "Moedor de pimenta e sal",
                Description = "Moedor elegante para pimenta e sal",

            },
            new Gift
            {
                Id = 21,
                Name = "Petisqueira",
                Description = "Petisqueira para servir aperitivos e petiscos",

            },
            new Gift
            {
                Id = 22,
                Name = "Cepo universal para facas inox",
                Description = "Suporte universal em inox para organizar facas",

            },
            new Gift
            {
                Id = 23,
                Name = "Conjunto de três peças para banheiro cerâmica branco",
                Description = "Kit com porta sabonete líquido, porta escova e bandeja em cerâmica branca",

            },
            new Gift
            {
                Id = 24,
                Name = "Kit de 4 tigelas de porcelana",
                Description = "Conjunto de 4 tigelas em porcelana para diversos usos",

            },
            new Gift
            {
                Id = 25,
                Name = "Conjunto de sousplat bordado 6 pessoas",
                Description = "Sousplats bordados elegantes para 6 pessoas",

            },
            new Gift
            {
                Id = 26,
                Name = "Boleira de vidro com tampa",
                Description = "Boleira em vidro transparente com tampa para bolos e doces",

            },
            new Gift
            {
                Id = 27,
                Name = "Cobre leito queen 400 fios/ cores neutras",
                Description = "Cobre leito queen de alta qualidade, 400 fios, cores neutras",

            },
            new Gift
            {
                Id = 28,
                Name = "Kit de potes herméticos/ 10 ou 12 unidades",
                Description = "Conjunto de potes herméticos para armazenamento",

            },
            new Gift
            {
                Id = 29,
                Name = "Forno de embutir elétrico preto 60 L com timer Dako Diplomata 220 V",
                Description = "Forno elétrico de embutir 60L com timer, 220V",

            },
            new Gift
            {
                Id = 30,
                Name = "Panela de cozimento a vapor",
                Description = "Panela especial para cozimento saudável a vapor",

            },
            new Gift
            {
                Id = 31,
                Name = "Fôrma de pudim",
                Description = "Fôrma especial para preparar pudins deliciosos",

            },
            new Gift
            {
                Id = 32,
                Name = "Chuveiro elétrico de parede",
                Description = "Chuveiro elétrico para instalação na parede",

            },
            new Gift
            {
                Id = 33,
                Name = "Jogo americano para seis pessoas/ cores neutras",
                Description = "Conjunto de jogos americanos para 6 pessoas em cores neutras",

            },
            new Gift
            {
                Id = 34,
                Name = "Pegador inox com ponta em silicone 23 cm",
                Description = "Pegador em inox com ponta de silicone de 23 cm",

            },
            new Gift
            {
                Id = 35,
                Name = "Mixer 3 em 1, 220v Batedor com Copo Medidor Triturador Lâminas em Inox 250W",
                Description = "Mixer 3 em 1 com batedor, copo medidor e triturador, 250W, 220V",

            }
        );
    }
}
