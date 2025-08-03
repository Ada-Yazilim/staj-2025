using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SigortaYonetimAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DURUM_TANIMLARI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tablo_adi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    alan_adi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    deger_kodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    deger_aciklama = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    siralama = table.Column<int>(type: "int", nullable: false),
                    aktif_mi = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DURUM_TA__3213E83F7B05CF3F", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SIGORTA_SIRKETLERI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sirket_adi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sirket_kodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    vergi_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    adres = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    aktif_mi = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    komisyon_orani = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    sozlesme_baslangic = table.Column<DateTime>(type: "datetime", nullable: true),
                    sozlesme_bitis = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SIGORTA___3213E83FD9ED5039", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KULLANICILAR",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sifre_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    email_dogrulandi = table.Column<bool>(type: "bit", nullable: false),
                    telefon_dogrulandi = table.Column<bool>(type: "bit", nullable: false),
                    son_giris_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    basarisiz_giris_sayisi = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    hesap_kilitlenme_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    kayit_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KULLANIC__3213E83F9E2CBCE3", x => x.id);
                    table.ForeignKey(
                        name: "FK__KULLANICI__durum__412EB0B6",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "POLICE_TURLERI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kategori_id = table.Column<int>(type: "int", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "int", nullable: false),
                    urun_adi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    urun_kodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    zorunlu_mi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    min_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    max_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    min_sure_gun = table.Column<int>(type: "int", nullable: true),
                    max_sure_gun = table.Column<int>(type: "int", nullable: true),
                    risk_faktorleri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    aktif_mi = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__POLICE_T__3213E83F4A0077E7", x => x.id);
                    table.ForeignKey(
                        name: "FK__POLICE_TU__alt_k__797309D9",
                        column: x => x.alt_kategori_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLICE_TU__kateg__787EE5A0",
                        column: x => x.kategori_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailDogrulandi = table.Column<bool>(type: "bit", nullable: false),
                    TelefonDogrulandi = table.Column<bool>(type: "bit", nullable: false),
                    SonGirisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BasarisizGirisSayisi = table.Column<int>(type: "int", nullable: false),
                    HesapKilitlenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KullanicilarId = table.Column<int>(type: "int", nullable: true),
                    Pozisyon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Departman = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YoneticiId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SonAktiviteTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SonIpAdresi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_YoneticiId",
                        column: x => x.YoneticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_KULLANICILAR_KullanicilarId",
                        column: x => x.KullanicilarId,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BILDIRIMLER",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    alici_kullanici_id = table.Column<int>(type: "int", nullable: false),
                    bildirim_tipi_id = table.Column<int>(type: "int", nullable: true),
                    baslik = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    icerik = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    oncelik_id = table.Column<int>(type: "int", nullable: true),
                    okundu_mu = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    gonderim_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    okunma_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    hata_mesaji = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BILDIRIM__3213E83F5EDBC28A", x => x.id);
                    table.ForeignKey(
                        name: "FK__BILDIRIML__alici__489AC854",
                        column: x => x.alici_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__BILDIRIML__bildi__498EEC8D",
                        column: x => x.bildirim_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__BILDIRIML__durum__4B7734FF",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__BILDIRIML__oncel__4A8310C6",
                        column: x => x.oncelik_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DOGRULAMA_KODLARI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kullanici_id = table.Column<int>(type: "int", nullable: false),
                    kod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    tip_id = table.Column<int>(type: "int", nullable: false),
                    amac_id = table.Column<int>(type: "int", nullable: false),
                    son_kullanma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    kullanildi_mi = table.Column<bool>(type: "bit", nullable: false),
                    deneme_sayisi = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DOGRULAM__3213E83FC3C872D7", x => x.id);
                    table.ForeignKey(
                        name: "FK__DOGRULAMA__amac___4D94879B",
                        column: x => x.amac_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DOGRULAMA__kulla__4BAC3F29",
                        column: x => x.kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DOGRULAMA__tip_i__4CA06362",
                        column: x => x.tip_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MUSTERILER",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kullanici_id = table.Column<int>(type: "int", nullable: true),
                    musteri_no = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tc_kimlik_no = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    eposta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    dogum_tarihi = table.Column<DateOnly>(type: "date", nullable: true),
                    cinsiyet_id = table.Column<int>(type: "int", nullable: true),
                    medeni_durum_id = table.Column<int>(type: "int", nullable: true),
                    meslek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    egitim_durumu_id = table.Column<int>(type: "int", nullable: true),
                    aylik_gelir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    adres_il = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    adres_ilce = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    adres_mahalle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    adres_detay = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    posta_kodu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    not_bilgileri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    blacklist_mi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    blacklist_nedeni = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    kayit_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    kaydeden_kullanici = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DURUM_TANIMLARIid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MUSTERIL__3213E83FA69A30A2", x => x.id);
                    table.ForeignKey(
                        name: "FK_MUSTERILER_DURUM_TANIMLARI_DURUM_TANIMLARIid",
                        column: x => x.DURUM_TANIMLARIid,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__MUSTERILE__cinsi__6383C8BA",
                        column: x => x.cinsiyet_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__MUSTERILE__egiti__656C112C",
                        column: x => x.egitim_durumu_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__MUSTERILE__kulla__619B8048",
                        column: x => x.kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__MUSTERILE__meden__6477ECF3",
                        column: x => x.medeni_durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RAPOR_SABLONLARI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rapor_adi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rapor_tipi_id = table.Column<int>(type: "int", nullable: true),
                    sql_sorgusu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parametreler = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    cikti_formati_id = table.Column<int>(type: "int", nullable: true),
                    otomatik_mi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    zamanlama_id = table.Column<int>(type: "int", nullable: true),
                    aktif_mi = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    olusturan_kullanici_id = table.Column<int>(type: "int", nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RAPOR_SA__3213E83FC1E43606", x => x.id);
                    table.ForeignKey(
                        name: "FK__RAPOR_SAB__cikti__5224328E",
                        column: x => x.cikti_formati_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__RAPOR_SAB__olust__540C7B00",
                        column: x => x.olusturan_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__RAPOR_SAB__rapor__51300E55",
                        column: x => x.rapor_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__RAPOR_SAB__zaman__531856C7",
                        column: x => x.zamanlama_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SIFRE_SIFIRLAMA",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kullanici_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    son_kullanma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    kullanildi_mi = table.Column<bool>(type: "bit", nullable: false),
                    ip_adresi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SIFRE_SI__3213E83FB8C30544", x => x.id);
                    table.ForeignKey(
                        name: "FK__SIFRE_SIF__kulla__45F365D3",
                        column: x => x.kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SISTEM_LOGLARI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kullanici_id = table.Column<int>(type: "int", nullable: true),
                    islem_tipi_id = table.Column<int>(type: "int", nullable: true),
                    tablo_adi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    kayit_id = table.Column<int>(type: "int", nullable: true),
                    eski_deger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    yeni_deger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip_adresi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tarayici_bilgisi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    islem_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    aciklama = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SISTEM_L__3213E83FED74E8C7", x => x.id);
                    table.ForeignKey(
                        name: "FK__SISTEM_LO__islem__43D61337",
                        column: x => x.islem_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__SISTEM_LO__kulla__42E1EEFE",
                        column: x => x.kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MUSTERI_ILETISIM_TERCIHLERI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    musteri_id = table.Column<int>(type: "int", nullable: false),
                    email_bildirimi = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    sms_bildirimi = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    whatsapp_bildirimi = table.Column<bool>(type: "bit", nullable: false),
                    arama_bildirimi = table.Column<bool>(type: "bit", nullable: false),
                    pazarlama_onayi = table.Column<bool>(type: "bit", nullable: false),
                    guncelleme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MUSTERI___3213E83F77352483", x => x.id);
                    table.ForeignKey(
                        name: "FK__MUSTERI_I__muste__6E01572D",
                        column: x => x.musteri_id,
                        principalTable: "MUSTERILER",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "POLICE_TEKLIFLERI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teklif_no = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    musteri_id = table.Column<int>(type: "int", nullable: false),
                    police_turu_id = table.Column<int>(type: "int", nullable: false),
                    sigorta_sirketi_id = table.Column<int>(type: "int", nullable: false),
                    olusturan_kullanici_id = table.Column<int>(type: "int", nullable: false),
                    risk_bilgileri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    teminat_bilgileri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    brut_prim = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    net_prim = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    komisyon_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    vergi_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    toplam_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    taksit_sayisi = table.Column<int>(type: "int", nullable: true),
                    teklif_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    gecerlilik_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    red_nedeni = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    onay_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    onaylayan_kullanici = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    notlar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__POLICE_T__3213E83F46F858E7", x => x.id);
                    table.ForeignKey(
                        name: "FK__POLICE_TE__durum__03F0984C",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLICE_TE__muste__00200768",
                        column: x => x.musteri_id,
                        principalTable: "MUSTERILER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLICE_TE__olust__02FC7413",
                        column: x => x.olusturan_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLICE_TE__polic__01142BA1",
                        column: x => x.police_turu_id,
                        principalTable: "POLICE_TURLERI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLICE_TE__sigor__02084FDA",
                        column: x => x.sigorta_sirketi_id,
                        principalTable: "SIGORTA_SIRKETLERI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "POLISELER",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    police_no = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    teklif_id = table.Column<int>(type: "int", nullable: true),
                    musteri_id = table.Column<int>(type: "int", nullable: false),
                    police_turu_id = table.Column<int>(type: "int", nullable: false),
                    sigorta_sirketi_id = table.Column<int>(type: "int", nullable: false),
                    tanzim_eden_kullanici_id = table.Column<int>(type: "int", nullable: false),
                    risk_bilgileri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    teminat_bilgileri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    baslangic_tarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    bitis_tarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    brut_prim = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    net_prim = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    komisyon_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    vergi_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    toplam_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    taksit_sayisi = table.Column<int>(type: "int", nullable: true),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    iptal_nedeni = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    iptal_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    yenileme_hatirlatma_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    ozel_sartlar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    notlar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    tanzim_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__POLISELE__3213E83F0ECB3EC8", x => x.id);
                    table.ForeignKey(
                        name: "FK__POLISELER__durum__0E6E26BF",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLISELER__muste__0A9D95DB",
                        column: x => x.musteri_id,
                        principalTable: "MUSTERILER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLISELER__polic__0B91BA14",
                        column: x => x.police_turu_id,
                        principalTable: "POLICE_TURLERI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLISELER__sigor__0C85DE4D",
                        column: x => x.sigorta_sirketi_id,
                        principalTable: "SIGORTA_SIRKETLERI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLISELER__tanzi__0D7A0286",
                        column: x => x.tanzim_eden_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__POLISELER__tekli__09A971A2",
                        column: x => x.teklif_id,
                        principalTable: "POLICE_TEKLIFLERI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DOKUMANLAR",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iliski_id = table.Column<int>(type: "int", nullable: true),
                    musteri_id = table.Column<int>(type: "int", nullable: true),
                    police_id = table.Column<int>(type: "int", nullable: true),
                    iliski_tipi_id = table.Column<int>(type: "int", nullable: true),
                    dosya_adi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    orijinal_dosya_adi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    dosya_yolu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    dosya_tipi_id = table.Column<int>(type: "int", nullable: true),
                    dosya_boyutu = table.Column<int>(type: "int", nullable: true),
                    dosya_boyutu_long = table.Column<long>(type: "bigint", nullable: true),
                    mime_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    hash_degeri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    kategori_id = table.Column<int>(type: "int", nullable: true),
                    dosya_turu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    aciklama = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    zorunlu_mu = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    yukleyen_kullanici_id = table.Column<int>(type: "int", nullable: true),
                    yuklenme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    yukleme_tarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    son_erisim_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    silinebilir_mi = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    gizlilik_seviyesi_id = table.Column<int>(type: "int", nullable: true),
                    musteriid = table.Column<int>(type: "int", nullable: true),
                    policeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DOKUMANL__3213E83F60EA8DE3", x => x.id);
                    table.ForeignKey(
                        name: "FK_DOKUMANLAR_MUSTERILER_musteriid",
                        column: x => x.musteriid,
                        principalTable: "MUSTERILER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DOKUMANLAR_POLISELER_policeid",
                        column: x => x.policeid,
                        principalTable: "POLISELER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DOKUMANLA__dosya__3C34F16F",
                        column: x => x.dosya_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DOKUMANLA__gizli__3E1D39E1",
                        column: x => x.gizlilik_seviyesi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DOKUMANLA__ilisk__3B40CD36",
                        column: x => x.iliski_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DOKUMANLA__kateg__3D2915A8",
                        column: x => x.kategori_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__DOKUMANLA__yukle__3F115E1A",
                        column: x => x.yukleyen_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HASAR_DOSYALAR",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hasar_no = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    dosya_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    police_id = table.Column<int>(type: "int", nullable: false),
                    musteri_id = table.Column<int>(type: "int", nullable: false),
                    bildiren_kullanici_id = table.Column<int>(type: "int", nullable: false),
                    olay_tarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    olay_saati = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    olay_yeri_il = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    olay_yeri_ilce = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    olay_yeri_detay = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    olay_tipi_id = table.Column<int>(type: "int", nullable: true),
                    olay_aciklamasi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    polis_rapor_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    polis_raporu_var_mi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    tanik_bilgileri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    talep_edilen_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ekspertiz_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    onaylanan_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    odenen_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    toplam_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    red_nedeni = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    sorumlu_eksper_id = table.Column<int>(type: "int", nullable: true),
                    ekspertiz_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    karar_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    odeme_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    onaylayan_kullanici = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    bildirim_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notlar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HASAR_DO__3213E83F9FD228F4", x => x.id);
                    table.ForeignKey(
                        name: "FK__HASAR_DOS__bildi__17036CC0",
                        column: x => x.bildiren_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__HASAR_DOS__durum__18EBB532",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__HASAR_DOS__muste__160F4887",
                        column: x => x.musteri_id,
                        principalTable: "MUSTERILER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__HASAR_DOS__olay___17F790F9",
                        column: x => x.olay_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__HASAR_DOS__polic__151B244E",
                        column: x => x.police_id,
                        principalTable: "POLISELER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__HASAR_DOS__sorum__19DFD96B",
                        column: x => x.sorumlu_eksper_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "KOMISYON_HESAPLARI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    police_id = table.Column<int>(type: "int", nullable: false),
                    acente_kullanici_id = table.Column<int>(type: "int", nullable: false),
                    brut_prim = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    komisyon_orani = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    komisyon_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    stopaj_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    net_komisyon = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    hesaplama_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    odeme_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    aciklama = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KOMISYON__3213E83F1FB37110", x => x.id);
                    table.ForeignKey(
                        name: "FK__KOMISYON___acent__3493CFA7",
                        column: x => x.acente_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__KOMISYON___durum__3587F3E0",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__KOMISYON___polic__339FAB6E",
                        column: x => x.police_id,
                        principalTable: "POLISELER",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "HASAR_TAKIP_NOTLARI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hasar_id = table.Column<int>(type: "int", nullable: false),
                    kullanici_id = table.Column<int>(type: "int", nullable: false),
                    not_metni = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    not_tipi_id = table.Column<int>(type: "int", nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HASAR_TA__3213E83F95ED626A", x => x.id);
                    table.ForeignKey(
                        name: "FK__HASAR_TAK__hasar__1DB06A4F",
                        column: x => x.hasar_id,
                        principalTable: "HASAR_DOSYALAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__HASAR_TAK__kulla__1EA48E88",
                        column: x => x.kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__HASAR_TAK__not_t__1F98B2C1",
                        column: x => x.not_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ODEMELER",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    odeme_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    taksit_id = table.Column<int>(type: "int", nullable: true),
                    police_id = table.Column<int>(type: "int", nullable: true),
                    musteri_id = table.Column<int>(type: "int", nullable: false),
                    tahsilat_yapan_kullanici_id = table.Column<int>(type: "int", nullable: true),
                    odeme_tipi_id = table.Column<int>(type: "int", nullable: true),
                    odeme_turu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    odeme_tarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vade_tarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    odeme_yontemi_id = table.Column<int>(type: "int", nullable: true),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    banka_adi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    kart_son_4_hane = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    pos_referans_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    banka_referans_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    makbuz_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    komisyon_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    aciklama = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    tahsil_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    muhasebe_kayit_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    muhasebede_kaydedildi_mi = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    fis_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    taksit_sayisi = table.Column<int>(type: "int", nullable: true),
                    taksit_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ODEMELER__3213E83F191C5CE6", x => x.id);
                    table.ForeignKey(
                        name: "FK__ODEMELER__durum___2FCF1A8A",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__ODEMELER__muster__2BFE89A6",
                        column: x => x.musteri_id,
                        principalTable: "MUSTERILER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__ODEMELER__odeme___2DE6D218",
                        column: x => x.odeme_tipi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__ODEMELER__odeme___2EDAF651",
                        column: x => x.odeme_yontemi_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__ODEMELER__police__2B0A656D",
                        column: x => x.police_id,
                        principalTable: "POLISELER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__ODEMELER__tahsil__2CF2ADDF",
                        column: x => x.tahsilat_yapan_kullanici_id,
                        principalTable: "KULLANICILAR",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TAKSITLER",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    odeme_id = table.Column<int>(type: "int", nullable: true),
                    police_id = table.Column<int>(type: "int", nullable: false),
                    taksit_no = table.Column<int>(type: "int", nullable: false),
                    vade_tarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    ana_para = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    vergi_tutari = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    toplam_tutar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    durum_id = table.Column<int>(type: "int", nullable: false),
                    odeme_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    gecikme_faizi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    gecikme_gun_sayisi = table.Column<int>(type: "int", nullable: true),
                    hatirlatma_durumu_id = table.Column<int>(type: "int", nullable: true),
                    son_hatirlatma_tarihi = table.Column<DateTime>(type: "datetime", nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    odemeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TAKSITLE__3213E83FE1900BE8", x => x.id);
                    table.ForeignKey(
                        name: "FK_TAKSITLER_ODEMELER_odemeid",
                        column: x => x.odemeid,
                        principalTable: "ODEMELER",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__TAKSITLER__durum__245D67DE",
                        column: x => x.durum_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__TAKSITLER__hatir__25518C17",
                        column: x => x.hatirlatma_durumu_id,
                        principalTable: "DURUM_TANIMLARI",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__TAKSITLER__polic__236943A5",
                        column: x => x.police_id,
                        principalTable: "POLISELER",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_KullanicilarId",
                table: "AspNetUsers",
                column: "KullanicilarId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_YoneticiId",
                table: "AspNetUsers",
                column: "YoneticiId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BILDIRIMLER_alici_kullanici_id",
                table: "BILDIRIMLER",
                column: "alici_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_BILDIRIMLER_bildirim_tipi_id",
                table: "BILDIRIMLER",
                column: "bildirim_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_BILDIRIMLER_durum_id",
                table: "BILDIRIMLER",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "IX_BILDIRIMLER_oncelik_id",
                table: "BILDIRIMLER",
                column: "oncelik_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOGRULAMA_KODLARI_amac_id",
                table: "DOGRULAMA_KODLARI",
                column: "amac_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOGRULAMA_KODLARI_kullanici_id",
                table: "DOGRULAMA_KODLARI",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOGRULAMA_KODLARI_tip_id",
                table: "DOGRULAMA_KODLARI",
                column: "tip_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOKUMANLAR_dosya_tipi_id",
                table: "DOKUMANLAR",
                column: "dosya_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOKUMANLAR_gizlilik_seviyesi_id",
                table: "DOKUMANLAR",
                column: "gizlilik_seviyesi_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOKUMANLAR_iliski_tipi_id",
                table: "DOKUMANLAR",
                column: "iliski_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOKUMANLAR_kategori_id",
                table: "DOKUMANLAR",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "IX_DOKUMANLAR_musteriid",
                table: "DOKUMANLAR",
                column: "musteriid");

            migrationBuilder.CreateIndex(
                name: "IX_DOKUMANLAR_policeid",
                table: "DOKUMANLAR",
                column: "policeid");

            migrationBuilder.CreateIndex(
                name: "IX_DOKUMANLAR_yukleyen_kullanici_id",
                table: "DOKUMANLAR",
                column: "yukleyen_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_DOSYALAR_bildiren_kullanici_id",
                table: "HASAR_DOSYALAR",
                column: "bildiren_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_DOSYALAR_durum_id",
                table: "HASAR_DOSYALAR",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_DOSYALAR_musteri_id",
                table: "HASAR_DOSYALAR",
                column: "musteri_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_DOSYALAR_olay_tipi_id",
                table: "HASAR_DOSYALAR",
                column: "olay_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_DOSYALAR_police_id",
                table: "HASAR_DOSYALAR",
                column: "police_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_DOSYALAR_sorumlu_eksper_id",
                table: "HASAR_DOSYALAR",
                column: "sorumlu_eksper_id");

            migrationBuilder.CreateIndex(
                name: "UQ__HASAR_DO__59C8A1EAB87CF833",
                table: "HASAR_DOSYALAR",
                column: "hasar_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_TAKIP_NOTLARI_hasar_id",
                table: "HASAR_TAKIP_NOTLARI",
                column: "hasar_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_TAKIP_NOTLARI_kullanici_id",
                table: "HASAR_TAKIP_NOTLARI",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_HASAR_TAKIP_NOTLARI_not_tipi_id",
                table: "HASAR_TAKIP_NOTLARI",
                column: "not_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_KOMISYON_HESAPLARI_acente_kullanici_id",
                table: "KOMISYON_HESAPLARI",
                column: "acente_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_KOMISYON_HESAPLARI_durum_id",
                table: "KOMISYON_HESAPLARI",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "IX_KOMISYON_HESAPLARI_police_id",
                table: "KOMISYON_HESAPLARI",
                column: "police_id");

            migrationBuilder.CreateIndex(
                name: "IX_KULLANICILAR_durum_id",
                table: "KULLANICILAR",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "UQ__KULLANIC__2549FEE6C5DAC599",
                table: "KULLANICILAR",
                column: "eposta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MUSTERI_ILETISIM_TERCIHLERI_musteri_id",
                table: "MUSTERI_ILETISIM_TERCIHLERI",
                column: "musteri_id");

            migrationBuilder.CreateIndex(
                name: "IX_MUSTERILER_cinsiyet_id",
                table: "MUSTERILER",
                column: "cinsiyet_id");

            migrationBuilder.CreateIndex(
                name: "IX_MUSTERILER_DURUM_TANIMLARIid",
                table: "MUSTERILER",
                column: "DURUM_TANIMLARIid");

            migrationBuilder.CreateIndex(
                name: "IX_MUSTERILER_egitim_durumu_id",
                table: "MUSTERILER",
                column: "egitim_durumu_id");

            migrationBuilder.CreateIndex(
                name: "IX_MUSTERILER_kullanici_id",
                table: "MUSTERILER",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_MUSTERILER_medeni_durum_id",
                table: "MUSTERILER",
                column: "medeni_durum_id");

            migrationBuilder.CreateIndex(
                name: "UQ__MUSTERIL__966D2E8C6CAD6128",
                table: "MUSTERILER",
                column: "musteri_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ODEMELER_durum_id",
                table: "ODEMELER",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "IX_ODEMELER_musteri_id",
                table: "ODEMELER",
                column: "musteri_id");

            migrationBuilder.CreateIndex(
                name: "IX_ODEMELER_odeme_tipi_id",
                table: "ODEMELER",
                column: "odeme_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_ODEMELER_odeme_yontemi_id",
                table: "ODEMELER",
                column: "odeme_yontemi_id");

            migrationBuilder.CreateIndex(
                name: "IX_ODEMELER_police_id",
                table: "ODEMELER",
                column: "police_id");

            migrationBuilder.CreateIndex(
                name: "IX_ODEMELER_tahsilat_yapan_kullanici_id",
                table: "ODEMELER",
                column: "tahsilat_yapan_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_ODEMELER_taksit_id",
                table: "ODEMELER",
                column: "taksit_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLICE_TEKLIFLERI_durum_id",
                table: "POLICE_TEKLIFLERI",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLICE_TEKLIFLERI_musteri_id",
                table: "POLICE_TEKLIFLERI",
                column: "musteri_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLICE_TEKLIFLERI_olusturan_kullanici_id",
                table: "POLICE_TEKLIFLERI",
                column: "olusturan_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLICE_TEKLIFLERI_police_turu_id",
                table: "POLICE_TEKLIFLERI",
                column: "police_turu_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLICE_TEKLIFLERI_sigorta_sirketi_id",
                table: "POLICE_TEKLIFLERI",
                column: "sigorta_sirketi_id");

            migrationBuilder.CreateIndex(
                name: "UQ__POLICE_T__A90A1AD96B121A3B",
                table: "POLICE_TEKLIFLERI",
                column: "teklif_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POLICE_TURLERI_alt_kategori_id",
                table: "POLICE_TURLERI",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLICE_TURLERI_kategori_id",
                table: "POLICE_TURLERI",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "UQ__POLICE_T__1359E5D30D80A072",
                table: "POLICE_TURLERI",
                column: "urun_kodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POLISELER_durum_id",
                table: "POLISELER",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLISELER_musteri_id",
                table: "POLISELER",
                column: "musteri_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLISELER_police_turu_id",
                table: "POLISELER",
                column: "police_turu_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLISELER_sigorta_sirketi_id",
                table: "POLISELER",
                column: "sigorta_sirketi_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLISELER_tanzim_eden_kullanici_id",
                table: "POLISELER",
                column: "tanzim_eden_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_POLISELER_teklif_id",
                table: "POLISELER",
                column: "teklif_id");

            migrationBuilder.CreateIndex(
                name: "UQ__POLISELE__68FFBEFB8254C983",
                table: "POLISELER",
                column: "police_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RAPOR_SABLONLARI_cikti_formati_id",
                table: "RAPOR_SABLONLARI",
                column: "cikti_formati_id");

            migrationBuilder.CreateIndex(
                name: "IX_RAPOR_SABLONLARI_olusturan_kullanici_id",
                table: "RAPOR_SABLONLARI",
                column: "olusturan_kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_RAPOR_SABLONLARI_rapor_tipi_id",
                table: "RAPOR_SABLONLARI",
                column: "rapor_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_RAPOR_SABLONLARI_zamanlama_id",
                table: "RAPOR_SABLONLARI",
                column: "zamanlama_id");

            migrationBuilder.CreateIndex(
                name: "IX_SIFRE_SIFIRLAMA_kullanici_id",
                table: "SIFRE_SIFIRLAMA",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "UQ__SIGORTA___C10CA9E75367FF1C",
                table: "SIGORTA_SIRKETLERI",
                column: "sirket_kodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SISTEM_LOGLARI_islem_tipi_id",
                table: "SISTEM_LOGLARI",
                column: "islem_tipi_id");

            migrationBuilder.CreateIndex(
                name: "IX_SISTEM_LOGLARI_kullanici_id",
                table: "SISTEM_LOGLARI",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_TAKSITLER_durum_id",
                table: "TAKSITLER",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "IX_TAKSITLER_hatirlatma_durumu_id",
                table: "TAKSITLER",
                column: "hatirlatma_durumu_id");

            migrationBuilder.CreateIndex(
                name: "IX_TAKSITLER_odemeid",
                table: "TAKSITLER",
                column: "odemeid");

            migrationBuilder.CreateIndex(
                name: "IX_TAKSITLER_police_id",
                table: "TAKSITLER",
                column: "police_id");

            migrationBuilder.AddForeignKey(
                name: "FK__ODEMELER__taksit__2A164134",
                table: "ODEMELER",
                column: "taksit_id",
                principalTable: "TAKSITLER",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__MUSTERILE__kulla__619B8048",
                table: "MUSTERILER");

            migrationBuilder.DropForeignKey(
                name: "FK__ODEMELER__tahsil__2CF2ADDF",
                table: "ODEMELER");

            migrationBuilder.DropForeignKey(
                name: "FK__POLICE_TE__olust__02FC7413",
                table: "POLICE_TEKLIFLERI");

            migrationBuilder.DropForeignKey(
                name: "FK__POLISELER__tanzi__0D7A0286",
                table: "POLISELER");

            migrationBuilder.DropForeignKey(
                name: "FK_MUSTERILER_DURUM_TANIMLARI_DURUM_TANIMLARIid",
                table: "MUSTERILER");

            migrationBuilder.DropForeignKey(
                name: "FK__MUSTERILE__cinsi__6383C8BA",
                table: "MUSTERILER");

            migrationBuilder.DropForeignKey(
                name: "FK__MUSTERILE__egiti__656C112C",
                table: "MUSTERILER");

            migrationBuilder.DropForeignKey(
                name: "FK__MUSTERILE__meden__6477ECF3",
                table: "MUSTERILER");

            migrationBuilder.DropForeignKey(
                name: "FK__ODEMELER__durum___2FCF1A8A",
                table: "ODEMELER");

            migrationBuilder.DropForeignKey(
                name: "FK__ODEMELER__odeme___2DE6D218",
                table: "ODEMELER");

            migrationBuilder.DropForeignKey(
                name: "FK__ODEMELER__odeme___2EDAF651",
                table: "ODEMELER");

            migrationBuilder.DropForeignKey(
                name: "FK__POLICE_TE__durum__03F0984C",
                table: "POLICE_TEKLIFLERI");

            migrationBuilder.DropForeignKey(
                name: "FK__POLICE_TU__alt_k__797309D9",
                table: "POLICE_TURLERI");

            migrationBuilder.DropForeignKey(
                name: "FK__POLICE_TU__kateg__787EE5A0",
                table: "POLICE_TURLERI");

            migrationBuilder.DropForeignKey(
                name: "FK__POLISELER__durum__0E6E26BF",
                table: "POLISELER");

            migrationBuilder.DropForeignKey(
                name: "FK__TAKSITLER__durum__245D67DE",
                table: "TAKSITLER");

            migrationBuilder.DropForeignKey(
                name: "FK__TAKSITLER__hatir__25518C17",
                table: "TAKSITLER");

            migrationBuilder.DropForeignKey(
                name: "FK__ODEMELER__muster__2BFE89A6",
                table: "ODEMELER");

            migrationBuilder.DropForeignKey(
                name: "FK__POLICE_TE__muste__00200768",
                table: "POLICE_TEKLIFLERI");

            migrationBuilder.DropForeignKey(
                name: "FK__POLISELER__muste__0A9D95DB",
                table: "POLISELER");

            migrationBuilder.DropForeignKey(
                name: "FK__ODEMELER__police__2B0A656D",
                table: "ODEMELER");

            migrationBuilder.DropForeignKey(
                name: "FK__TAKSITLER__polic__236943A5",
                table: "TAKSITLER");

            migrationBuilder.DropForeignKey(
                name: "FK__ODEMELER__taksit__2A164134",
                table: "ODEMELER");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BILDIRIMLER");

            migrationBuilder.DropTable(
                name: "DOGRULAMA_KODLARI");

            migrationBuilder.DropTable(
                name: "DOKUMANLAR");

            migrationBuilder.DropTable(
                name: "HASAR_TAKIP_NOTLARI");

            migrationBuilder.DropTable(
                name: "KOMISYON_HESAPLARI");

            migrationBuilder.DropTable(
                name: "MUSTERI_ILETISIM_TERCIHLERI");

            migrationBuilder.DropTable(
                name: "RAPOR_SABLONLARI");

            migrationBuilder.DropTable(
                name: "SIFRE_SIFIRLAMA");

            migrationBuilder.DropTable(
                name: "SISTEM_LOGLARI");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "HASAR_DOSYALAR");

            migrationBuilder.DropTable(
                name: "KULLANICILAR");

            migrationBuilder.DropTable(
                name: "DURUM_TANIMLARI");

            migrationBuilder.DropTable(
                name: "MUSTERILER");

            migrationBuilder.DropTable(
                name: "POLISELER");

            migrationBuilder.DropTable(
                name: "POLICE_TEKLIFLERI");

            migrationBuilder.DropTable(
                name: "POLICE_TURLERI");

            migrationBuilder.DropTable(
                name: "SIGORTA_SIRKETLERI");

            migrationBuilder.DropTable(
                name: "TAKSITLER");

            migrationBuilder.DropTable(
                name: "ODEMELER");
        }
    }
}
