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
            new Gift { Id = 1, Name = "Organizadores de armário", Description = "Conjunto de peças em acrílico para organizar prateleiras do armário de cozinha" },
            new Gift { Id = 2, Name = "Aparelho de jantar", Description = "Aparelho de jantar em porcelana branca para servir refeições" },
            new Gift { Id = 3, Name = "Travessas", Description = "Conjunto de travessas brancas em porcelana para servir alimentos" },
            new Gift { Id = 4, Name = "Pratos de sobremesa", Description = "Pratos pequenos de porcelana branca para sobremesas e aperitivos" },
            new Gift { Id = 5, Name = "Talheres sobremesa", Description = "Conjunto de talheres específicos para uso em sobremesas" },
            new Gift { Id = 6, Name = "Tapetes de banheiro", Description = "Tapetes em cores neutras, absorventes e antiderrapantes" },
            new Gift { Id = 7, Name = "Tábua de carne", Description = "Tábua de madeira resistente para corte de carnes e outros alimentos" },
            new Gift { Id = 8, Name = "Toalha de mesa", Description = "Toalha em cores neutras para mesa de 6 lugares" },
            new Gift { Id = 9, Name = "Jogo de cama queen", Description = "Jogo completo em tecido 400 fios, para cama casal queen, cores neutras" },
            new Gift { Id = 10, Name = "Talheres de servir", Description = "Conjunto de talheres em inox para servir saladas, massas e pratos principais" },
            new Gift { Id = 11, Name = "Lixeiras inox", Description = "Par de lixeiras em aço inoxidável para banheiros" },
            new Gift { Id = 12, Name = "Varal de chão", Description = "Varal dobrável para roupas, portátil e fácil de armazenar" },
            new Gift { Id = 13, Name = "Jarra de cristal", Description = "Jarra transparente em cristal para servir água, sucos ou vinhos" },
            new Gift { Id = 14, Name = "Saleiro", Description = "Recipiente em cerâmica para armazenamento e uso de sal" },
            new Gift { Id = 15, Name = "Açucareiro", Description = "Recipiente em cerâmica para armazenar açúcar e servir junto ao café" },
            new Gift { Id = 16, Name = "Pipoqueira elétrica", Description = "Aparelho elétrico para preparo rápido de pipoca sem óleo" },
            new Gift { Id = 17, Name = "Panela wok", Description = "Panela wok com revestimento cerâmico antiaderente" },
            new Gift { Id = 18, Name = "Moedor sal e pimenta", Description = "Moedor manual ajustável para grãos de pimenta e sal grosso" },
            new Gift { Id = 19, Name = "Petisqueira", Description = "Peça com divisórias para servir aperitivos e petiscos" },
            new Gift { Id = 20, Name = "Cepo para facas", Description = "Suporte universal em inox para armazenar diferentes tipos de facas" },
            new Gift { Id = 21, Name = "Kit banheiro cerâmica", Description = "Conjunto com porta sabonete líquido, porta escova e bandeja, em cerâmica branca" },
            new Gift { Id = 22, Name = "Tigelas porcelana", Description = "Conjunto com 4 tigelas em porcelana para servir caldos, sopas, cereais etc." },
            new Gift { Id = 23, Name = "Sousplat bordado", Description = "Conjunto com 6 sousplats bordados para montagem de mesa" },
            new Gift { Id = 24, Name = "Boleira vidro", Description = "Boleira em vidro transparente com tampa para bolos" },
            new Gift { Id = 25, Name = "Cobre leito queen", Description = "Cobre leito em tecido 400 fios, tamanho queen, cores neutras" },
            new Gift { Id = 26, Name = "Potes herméticos", Description = "Kit com 10 a 12 potes herméticos transparentes para armazenar alimentos" },
            new Gift { Id = 27, Name = "Forno de embutir", Description = "Forno elétrico preto de 60L, modelo Dako Diplomata, com timer, 220V" },
            new Gift { Id = 28, Name = "Panela a vapor", Description = "Panela própria para preparo de alimentos no vapor" },
            new Gift { Id = 29, Name = "Forma de pudim", Description = "Forma redonda com furo central, indicada para pudins e sobremesas similares" },
            new Gift { Id = 30, Name = "Chuveiro elétrico", Description = "Chuveiro de parede, modelo elétrico, para uso em 220V" },
            new Gift { Id = 31, Name = "Jogo americano", Description = "Conjunto de jogos americanos para 6 pessoas, em cores neutras" },
            new Gift { Id = 32, Name = "Pegador inox", Description = "Pegador de inox com ponta em silicone, 23 cm" },
            new Gift { Id = 33, Name = "Mixer 3 em 1", Description = "Aparelho com função mixer, batedor e triturador, acompanha copo medidor, 250W, 220V" },
            new Gift { Id = 34, Name = "Cortina pequena", Description = "Cortina voil com forro microfibra, cor creme, medidas 1.65 m x 2.27 m" },
            new Gift { Id = 35, Name = "Cortina grande", Description = "Cortina voil com forro microfibra, cor creme, medidas 2.90 m x 2.27 m" },
            new Gift { Id = 36, Name = "Persiana cozinha", Description = "Persiana branca para cozinha, medidas 2.37 m x 67 cm" },
            new Gift { Id = 37, Name = "Balança de cozinha", Description = "Balança digital compacta com visor, capacidade de 1 g a 10 kg" },
            new Gift { Id = 38, Name = "Utensílios de cozinha em silicone", Description = "Conjunto contendo espátula vazada, colher, espátula reta e espátula abaulada, resistente ao calor" },
            new Gift { Id = 39, Name = "Formas de air fryer", Description = "Kit 2 formas de silicone para air fryer com alça antiaderente, reutilizável, resistente ao calor" },
            new Gift { Id = 40, Name = "Porta guardanapos", Description = "Porta guardanapos de cristal" },
            new Gift { Id = 41, Name = "Alexa", Description = "Echo Dot (Geração mais recente)" },
            new Gift { Id = 42, Name = "Robô aspirador", Description = "Robô Aspirador Passa Pano KaBuM! smart 900 (com mapeamento 3D)" }
        );
    }
}
