import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import './Dashboard.css';

interface Kullanici {
  id: number;
  ad: string;
  soyad: string;
  eposta: string;
  telefon: string;
  durum_id: number;
  email_dogrulandi: boolean;
  telefon_dogrulandi: boolean;
  kayit_tarihi: string;
  guncelleme_tarihi: string;
}

interface AdminUser {
  id: string;
  ad: string;
  soyad: string;
  email: string;
  telefon: string;
  emailDogrulandi: boolean;
  telefonDogrulandi: boolean;
  kayitTarihi: string;
  guncellemeTarihi: string;
  sonGirisTarihi?: string;
  sonAktiviteTarihi?: string;
  sonIpAdresi?: string;
  basarisizGirisSayisi: number;
  hesapKilitlenmeTarihi?: string;
  aktifMi: boolean;
  pozisyon?: string;
  departman?: string;
  notlar?: string;
  kullanicilarId?: number;
  kullanicilarDurum?: number;
  musteriId?: number;
  musteriNo?: string;
  yoneticiId?: string;
  yoneticiAdi?: string;
  roller: string[];
  hesapKilitliMi: boolean;
  tamAd: string;
}

interface DashboardStats {
  totalUsers: number;
  activeUsers: number;
  lockedUsers: number;
  verifiedUsers: number;
  roleDistribution: { role: string; count: number }[];
  recentLogins: { ad: string; soyad: string; email: string; sonGirisTarihi: string }[];
}

interface SystemReport {
  userMetrics: {
    totalUsers: number;
    activeUsers: number;
    newUsersThisMonth: number;
    lockedAccounts: number;
    unverifiedEmails: number;
  };
  securityMetrics: {
    failedLoginAttempts: number;
    accountsLockedThisWeek: number;
    passwordResetsThisMonth: number;
  };
  systemActivity: {
    totalLogins: number;
    loginsThisWeek: number;
    averageSessionDuration: number;
    systemLogs: number;
  };
  departmentBreakdown: { department: string; count: number }[];
  roleDistribution: { role: string; count: number }[];
}

interface UserPerformance {
  toplamSatis: number;
  toplamPrim: number;
  toplamKomisyon: number;
  yeniMusteriler: number;
  hasarAdedi: number;
  girisGunleri: number;
}

interface UserAudit {
  loginHistory: Array<{
    giris_tarihi: string;
    cikis_tarihi?: string;
    ip_adresi: string;
    tarayici_bilgisi: string;
    oturum_suresi?: number;
  }>;
  systemLogs: Array<{
    islem_tarihi: string;
    islemTipi: string;
    tablo_adi: string;
    aciklama: string;
    ip_adresi: string;
  }>;
  securityEvents: {
    basarisizGirisler: number;
    sonBasarisizGiris?: string;
    sonSifreDegisiklik?: string;
  };
}

// Müşteri Modülü Interfaces
interface MusteriListDto {
  id: number;
  musteri_no: string;
  tip_adi: string;
  ad?: string;
  soyad?: string;
  sirket_adi?: string;
  eposta?: string;
  telefon?: string;
  adres_il?: string;
  blacklist_mi?: boolean;
  kayit_tarihi: string;
  tam_ad: string;
}

interface MusteriDetayDto {
  id: number;
  musteri_no: string;
  tip_id: number;
  tip_adi: string;
  ad?: string;
  soyad?: string;
  sirket_adi?: string;
  vergi_no?: string;
  tc_kimlik_no?: string;
  eposta?: string;
  telefon?: string;
  cep_telefonu?: string;
  dogum_tarihi?: string;
  cinsiyet_id?: number;
  cinsiyet_adi?: string;
  medeni_durum_id?: number;
  medeni_durum_adi?: string;
  meslek?: string;
  egitim_durumu_id?: number;
  egitim_durumu_adi?: string;
  aylik_gelir?: number;
  adres_il?: string;
  adres_ilce?: string;
  adres_mahalle?: string;
  adres_detay?: string;
  posta_kodu?: string;
  not_bilgileri?: string;
  blacklist_mi?: boolean;
  blacklist_nedeni?: string;
  kayit_tarihi: string;
  guncelleme_tarihi: string;
  kaydeden_kullanici?: string;
  toplam_police_sayisi: number;
  toplam_prim_tutari: number;
  aktif_police_sayisi: number;
  hasar_sayisi: number;
}

interface MusteriCreateDto {
  tip_id: number;
  ad?: string;
  soyad?: string;
  sirket_adi?: string;
  vergi_no?: string;
  tc_kimlik_no?: string;
  eposta?: string;
  telefon?: string;
  cep_telefonu?: string;
  dogum_tarihi?: string;
  cinsiyet_id?: number;
  medeni_durum_id?: number;
  meslek?: string;
  egitim_durumu_id?: number;
  aylik_gelir?: number;
  adres_il?: string;
  adres_ilce?: string;
  adres_mahalle?: string;
  adres_detay?: string;
  posta_kodu?: string;
  not_bilgileri?: string;
  blacklist_mi?: boolean;
  blacklist_nedeni?: string;
}

interface MusteriSearchDto {
  arama_metni?: string;
  tip_id?: number;
  musteri_no?: string;
  tc_kimlik_no?: string;
  vergi_no?: string;
  eposta?: string;
  telefon?: string;
  adres_il?: string;
  blacklist_mi?: boolean;
  kayit_tarihi_baslangic?: string;
  kayit_tarihi_bitis?: string;
  sayfa: number;
  sayfa_boyutu: number;
  siralama: string;
}

interface MusteriIstatistikDto {
  toplam_musteri_sayisi: number;
  bireysel_musteri_sayisi: number;
  kurumsal_musteri_sayisi: number;
  blacklist_musteri_sayisi: number;
  bu_ay_eklenen_sayisi: number;
  ortalama_aylık_gelir: number;
  il_bazinda_dagilim: Array<{
    il_adi: string;
    musteri_sayisi: number;
  }>;
}

interface LookupData {
  musteri_tipleri: Array<{id: number; text: string}>;
  cinsiyetler: Array<{id: number; text: string}>;
  medeni_durumlar: Array<{id: number; text: string}>;
  egitim_durumlari: Array<{id: number; text: string}>;
}

// 🔧 USERMODAL COMPONENT - Gelişmiş Kullanıcı Modal'ları
interface UserModalProps {
  type: 'view' | 'edit' | 'performance' | 'audit' | 'security';
  user: AdminUser;
  performance?: UserPerformance | null;
  audit?: UserAudit | null;
  onClose: () => void;
  onSave: (updatedUser: AdminUser) => void;
  onPasswordReset: (userId: string, newPassword: string) => void;
  onAddNote: (userId: string, note: string) => void;
}

const UserModal: React.FC<UserModalProps> = ({ 
  type, 
  user, 
  performance, 
  audit, 
  onClose, 
  onSave, 
  onPasswordReset, 
  onAddNote 
}) => {
  const [formData, setFormData] = useState({
    ad: user.ad,
    soyad: user.soyad,
    email: user.email,
    telefon: user.telefon,
    pozisyon: user.pozisyon || '',
    departman: user.departman || '',
    notlar: user.notlar || ''
  });
  const [newPassword, setNewPassword] = useState('');
  const [newNote, setNewNote] = useState('');

  const handleSave = () => {
    const updatedUser: AdminUser = {
      ...user,
      ...formData
    };
    onSave(updatedUser);
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>
            {type === 'view' && '👁️ Kullanıcı Detayları'}
            {type === 'edit' && '✏️ Kullanıcı Düzenle'}
            {type === 'performance' && '📊 Performans Raporu'}
            {type === 'audit' && '📋 Audit Log'}
            {type === 'security' && '🔒 Güvenlik Ayarları'}
          </h2>
          <button className="modal-close" onClick={onClose}>❌</button>
        </div>

        <div className="modal-body">
          {type === 'view' && (
            <div className="user-details">
              <div className="detail-section">
                <h3>👤 Temel Bilgiler</h3>
                <div className="detail-grid">
                  <div className="detail-item">
                    <label>Ad Soyad:</label>
                    <span>{user.ad} {user.soyad}</span>
                  </div>
                  <div className="detail-item">
                    <label>E-posta:</label>
                    <span>{user.email} {user.emailDogrulandi && <span className="verified">✅</span>}</span>
                  </div>
                  <div className="detail-item">
                    <label>Telefon:</label>
                    <span>{user.telefon || 'Belirtilmemiş'} {user.telefonDogrulandi && <span className="verified">✅</span>}</span>
                  </div>
                  <div className="detail-item">
                    <label>Pozisyon:</label>
                    <span>{user.pozisyon || 'Belirtilmemiş'}</span>
                  </div>
                  <div className="detail-item">
                    <label>Departman:</label>
                    <span>{user.departman || 'Belirtilmemiş'}</span>
                  </div>
                  <div className="detail-item">
                    <label>Roller:</label>
                    <div className="roles">
                      {user.roller.map(role => (
                        <span key={role} className={`role-badge ${role.toLowerCase()}`}>{role}</span>
                      ))}
                    </div>
                  </div>
                </div>
              </div>

              <div className="detail-section">
                <h3>📊 Durum Bilgileri</h3>
                <div className="detail-grid">
                  <div className="detail-item">
                    <label>Hesap Durumu:</label>
                    <span className={user.aktifMi ? 'status-active' : 'status-inactive'}>
                      {user.aktifMi ? '✅ Aktif' : '⏸️ Pasif'}
                    </span>
                  </div>
                  <div className="detail-item">
                    <label>Kilit Durumu:</label>
                    <span className={user.hesapKilitliMi ? 'status-locked' : 'status-unlocked'}>
                      {user.hesapKilitliMi ? '🔒 Kilitli' : '🔓 Açık'}
                    </span>
                  </div>
                  <div className="detail-item">
                    <label>Başarısız Giriş:</label>
                    <span>{user.basarisizGirisSayisi}</span>
                  </div>
                  <div className="detail-item">
                    <label>Son Giriş:</label>
                    <span>{user.sonGirisTarihi ? new Date(user.sonGirisTarihi).toLocaleString('tr-TR') : 'Hiç giriş yapmamış'}</span>
                  </div>
                  <div className="detail-item">
                    <label>Son IP:</label>
                    <span>{user.sonIpAdresi || 'Bilinmiyor'}</span>
                  </div>
                  <div className="detail-item">
                    <label>Kayıt Tarihi:</label>
                    <span>{new Date(user.kayitTarihi).toLocaleString('tr-TR')}</span>
                  </div>
                </div>
              </div>

              {user.notlar && (
                <div className="detail-section">
                  <h3>📝 Notlar</h3>
                  <div className="notes-content">
                    {user.notlar.split('\n').map((note, index) => (
                      <p key={index} className="note-item">{note}</p>
                    ))}
                  </div>
                </div>
              )}
            </div>
          )}

          {type === 'edit' && (
            <div className="edit-form">
              <div className="form-grid">
                <div className="form-group">
                  <label>👤 Ad:</label>
                  <input
                    type="text"
                    value={formData.ad}
                    onChange={(e) => setFormData({...formData, ad: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>👤 Soyad:</label>
                  <input
                    type="text"
                    value={formData.soyad}
                    onChange={(e) => setFormData({...formData, soyad: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>📧 E-posta:</label>
                  <input
                    type="email"
                    value={formData.email}
                    onChange={(e) => setFormData({...formData, email: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>📱 Telefon:</label>
                  <input
                    type="tel"
                    value={formData.telefon}
                    onChange={(e) => setFormData({...formData, telefon: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>💼 Pozisyon:</label>
                  <input
                    type="text"
                    value={formData.pozisyon}
                    onChange={(e) => setFormData({...formData, pozisyon: e.target.value})}
                  />
                </div>
                <div className="form-group">
                  <label>🏢 Departman:</label>
                  <select
                    value={formData.departman}
                    onChange={(e) => setFormData({...formData, departman: e.target.value})}
                  >
                    <option value="">Departman Seçin</option>
                    <option value="IT">IT</option>
                    <option value="Satış">Satış</option>
                    <option value="Muhasebe">Muhasebe</option>
                    <option value="İnsan Kaynakları">İnsan Kaynakları</option>
                    <option value="Operasyon">Operasyon</option>
                    <option value="Hasar">Hasar</option>
                  </select>
                </div>
              </div>
              
              <div className="form-group">
                <label>📝 Notlar:</label>
                <textarea
                  value={formData.notlar}
                  onChange={(e) => setFormData({...formData, notlar: e.target.value})}
                  rows={4}
                  placeholder="Kullanıcı ile ilgili notlar..."
                />
              </div>
            </div>
          )}

          {type === 'performance' && performance && (
            <div className="performance-report">
              <div className="performance-grid">
                <div className="performance-card">
                  <h4>💼 Satış Performansı</h4>
                  <div className="performance-metrics">
                    <div className="metric">
                      <span>Toplam Satış:</span>
                      <strong>{performance.toplamSatis}</strong>
                    </div>
                    <div className="metric">
                      <span>Toplam Prim:</span>
                      <strong>₺{performance.toplamPrim.toLocaleString('tr-TR')}</strong>
                    </div>
                    <div className="metric">
                      <span>Toplam Komisyon:</span>
                      <strong>₺{performance.toplamKomisyon.toLocaleString('tr-TR')}</strong>
                    </div>
                  </div>
                </div>

                <div className="performance-card">
                  <h4>👥 Müşteri Metrikleri</h4>
                  <div className="performance-metrics">
                    <div className="metric">
                      <span>Yeni Müşteriler:</span>
                      <strong>{performance.yeniMusteriler}</strong>
                    </div>
                    <div className="metric">
                      <span>Hasar Adedi:</span>
                      <strong>{performance.hasarAdedi}</strong>
                    </div>
                    <div className="metric">
                      <span>Aktif Giriş Günleri:</span>
                      <strong>{performance.girisGunleri}</strong>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {type === 'audit' && audit && (
            <div className="audit-report">
              <div className="audit-section">
                <h4>🕐 Giriş Geçmişi</h4>
                <div className="audit-list">
                  {audit.loginHistory.map((login, index) => (
                    <div key={index} className="audit-item">
                      <div className="audit-time">
                        {new Date(login.giris_tarihi).toLocaleString('tr-TR')}
                      </div>
                      <div className="audit-details">
                        <span>🌐 {login.ip_adresi}</span>
                        <span>🖥️ {login.tarayici_bilgisi}</span>
                        {login.oturum_suresi && <span>⏱️ {login.oturum_suresi} dk</span>}
                      </div>
                    </div>
                  ))}
                </div>
              </div>

              <div className="audit-section">
                <h4>📋 Sistem Aktiviteleri</h4>
                <div className="audit-list">
                  {audit.systemLogs.map((log, index) => (
                    <div key={index} className="audit-item">
                      <div className="audit-time">
                        {new Date(log.islem_tarihi).toLocaleString('tr-TR')}
                      </div>
                      <div className="audit-details">
                        <span className="log-type">{log.islemTipi}</span>
                        <span>{log.aciklama}</span>
                      </div>
                    </div>
                  ))}
                </div>
              </div>

              <div className="audit-section">
                <h4>🔒 Güvenlik Olayları</h4>
                <div className="security-events">
                  <div className="security-metric">
                    <span>Başarısız Girişler:</span>
                    <strong>{audit.securityEvents.basarisizGirisler}</strong>
                  </div>
                  {audit.securityEvents.sonBasarisizGiris && (
                    <div className="security-metric">
                      <span>Son Başarısız Giriş:</span>
                      <strong>{new Date(audit.securityEvents.sonBasarisizGiris).toLocaleString('tr-TR')}</strong>
                    </div>
                  )}
                  {audit.securityEvents.sonSifreDegisiklik && (
                    <div className="security-metric">
                      <span>Son Şifre Değişikliği:</span>
                      <strong>{new Date(audit.securityEvents.sonSifreDegisiklik).toLocaleString('tr-TR')}</strong>
                    </div>
                  )}
                </div>
              </div>
            </div>
          )}

          {type === 'security' && (
            <div className="security-settings">
              <div className="security-section">
                <h4>🔑 Şifre Yönetimi</h4>
                <div className="form-group">
                  <label>Yeni Şifre:</label>
                  <input
                    type="password"
                    value={newPassword}
                    onChange={(e) => setNewPassword(e.target.value)}
                    placeholder="Yeni şifre girin"
                  />
                  <button 
                    className="btn btn-warning"
                    onClick={() => {
                      if (newPassword.length >= 6) {
                        onPasswordReset(user.id, newPassword);
                        setNewPassword('');
                      } else {
                        alert('Şifre en az 6 karakter olmalıdır.');
                      }
                    }}
                  >
                    🔑 Şifreyi Sıfırla
                  </button>
                </div>
              </div>

              <div className="security-section">
                <h4>📝 Not Ekle</h4>
                <div className="form-group">
                  <textarea
                    value={newNote}
                    onChange={(e) => setNewNote(e.target.value)}
                    placeholder="Güvenlik notu ekleyin..."
                    rows={3}
                  />
                  <button 
                    className="btn btn-primary"
                    onClick={() => {
                      if (newNote.trim()) {
                        onAddNote(user.id, newNote);
                        setNewNote('');
                      }
                    }}
                  >
                    📝 Not Ekle
                  </button>
                </div>
              </div>

              <div className="security-section">
                <h4>⚙️ Güvenlik Ayarları</h4>
                <div className="security-options">
                  <label className="security-option">
                    <input type="checkbox" />
                    <span>İki faktörlü doğrulama zorunlu</span>
                  </label>
                  <label className="security-option">
                    <input type="checkbox" />
                    <span>Güçlü şifre zorunlu</span>
                  </label>
                  <label className="security-option">
                    <input type="checkbox" />
                    <span>Oturum zaman aşımı kısa</span>
                  </label>
                  <label className="security-option">
                    <input type="checkbox" />
                    <span>IP adresi kısıtlaması</span>
                  </label>
                </div>
              </div>
            </div>
          )}
        </div>

        <div className="modal-footer">
          {type === 'edit' && (
            <>
              <button className="btn btn-primary" onClick={handleSave}>
                💾 Kaydet
              </button>
              <button className="btn btn-secondary" onClick={onClose}>
                ❌ İptal
              </button>
            </>
          )}
          {type !== 'edit' && (
            <button className="btn btn-secondary" onClick={onClose}>
              ✅ Kapat
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

const Dashboard: React.FC = () => {
  const [kullanicilar, setKullanicilar] = useState<Kullanici[]>([]);
  const [adminUsers, setAdminUsers] = useState<AdminUser[]>([]);
  const [dashboardStats, setDashboardStats] = useState<DashboardStats | null>(null);
  const [systemReport, setSystemReport] = useState<SystemReport | null>(null);
  const [selectedUser, setSelectedUser] = useState<AdminUser | null>(null);
  const [userPerformance, setUserPerformance] = useState<UserPerformance | null>(null);
  const [userAudit, setUserAudit] = useState<UserAudit | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'overview' | 'users' | 'create' | 'reports' | 'security' | 'hierarchy' | 'customers' | 'sales' | 'commission' | 'policies' | 'claims'>('overview');
  const [userModal, setUserModal] = useState<{type: 'view' | 'edit' | 'performance' | 'audit' | 'security' | null, user?: AdminUser}>({type: null});
  const [filters, setFilters] = useState({
    search: '',
    role: '',
    status: '',
    department: '',
    position: ''
  });
  const [bulkSelection, setBulkSelection] = useState<string[]>([]);
  const [createUserForm, setCreateUserForm] = useState({
    ad: '',
    soyad: '',
    email: '',
    password: '',
    telefon: '',
    role: 'KULLANICI',
    pozisyon: '',
    departman: '',
    yoneticiId: ''
  });

  // Müşteri Modülü State'leri
  const [musteriler, setMusteriler] = useState<MusteriListDto[]>([]);
  const [selectedMusteri, setSelectedMusteri] = useState<MusteriDetayDto | null>(null);
  const [musteriIstatistikleri, setMusteriIstatistikleri] = useState<MusteriIstatistikDto | null>(null);
  const [lookupData, setLookupData] = useState<LookupData | null>(null);
  const [musteriSearchParams, setMusteriSearchParams] = useState<MusteriSearchDto>({
    sayfa: 1,
    sayfa_boyutu: 20,
    siralama: 'kayit_tarihi_desc'
  });
  const [musteriModal, setMusteriModal] = useState<{
    type: 'create' | 'edit' | 'view' | null;
    musteri?: MusteriDetayDto;
  }>({ type: null });
  const [musteriCreateForm, setMusteriCreateForm] = useState<MusteriCreateDto>({
    tip_id: 0,
    blacklist_mi: false
  });
  const [musteriTotalCount, setMusteriTotalCount] = useState(0);
  const [musteriLoading, setMusteriLoading] = useState(false);

  const { user, logout, token } = useAuth();

  const isAdmin = user?.roles?.includes('ADMIN');
  const isAcente = user?.roles?.includes('ACENTE');
  const isKullanici = user?.roles?.includes('KULLANICI');

  // Müşteri Modülü API Fonksiyonları
  const fetchMusteriler = async () => {
    try {
      setMusteriLoading(true);
      const queryParams = new URLSearchParams({
        sayfa: musteriSearchParams.sayfa.toString(),
        sayfa_boyutu: musteriSearchParams.sayfa_boyutu.toString(),
        siralama: musteriSearchParams.siralama,
        ...(musteriSearchParams.arama_metni && { arama_metni: musteriSearchParams.arama_metni }),
        ...(musteriSearchParams.tip_id && { tip_id: musteriSearchParams.tip_id.toString() }),
        ...(musteriSearchParams.blacklist_mi !== undefined && { blacklist_mi: musteriSearchParams.blacklist_mi.toString() }),
      });

      const response = await fetch(`http://localhost:5000/api/Musteriler?${queryParams}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const data = await response.json();
        if (Array.isArray(data.Data)) {
          setMusteriler(data.Data);
          setMusteriTotalCount(data.TotalCount || data.Data.length);
        } else if (Array.isArray(data)) {
          setMusteriler(data);
          setMusteriTotalCount(data.length);
        } else {
          setMusteriler([]);
          setMusteriTotalCount(0);
        }
      } else {
        setMusteriler([]);
        setMusteriTotalCount(0);
        console.error('Müşteriler getirilemedi:', response.status);
      }
    } catch (err) {
      setMusteriler([]);
      setMusteriTotalCount(0);
      console.error('Müşteri API hatası:', err);
    } finally {
      setMusteriLoading(false);
    }
  };

  const fetchMusteriDetay = async (musteriId: number) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Musteriler/${musteriId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const musteri = await response.json();
        setSelectedMusteri(musteri);
        return musteri;
      }
    } catch (err) {
      console.error('Müşteri detay getirme hatası:', err);
    }
  };

  const fetchMusteriIstatistikleri = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/Musteriler/istatistikler', {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const stats = await response.json();
        setMusteriIstatistikleri(stats);
      }
    } catch (err) {
      console.error('Müşteri istatistik hatası:', err);
    }
  };

  const fetchLookupData = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/Musteriler/lookup-data', {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const data = await response.json();
        setLookupData(data);
      }
    } catch (err) {
      console.error('Lookup data hatası:', err);
    }
  };

  const createMusteri = async (musteriData: MusteriCreateDto) => {
    try {
      const response = await fetch('http://localhost:5000/api/Musteriler', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(musteriData)
      });

      if (response.ok) {
        await fetchMusteriler();
        setMusteriModal({ type: null });
        setMusteriCreateForm({ tip_id: 0, blacklist_mi: false });
        return true;
      } else {
        const errorData = await response.json();
        console.error('Müşteri oluşturma hatası:', errorData);
        return false;
      }
    } catch (err) {
      console.error('Müşteri oluşturma API hatası:', err);
      return false;
    }
  };

  const updateMusteri = async (musteriData: MusteriCreateDto & { id: number }) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Musteriler/${musteriData.id}`, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(musteriData)
      });

      if (response.ok) {
        await fetchMusteriler();
        setMusteriModal({ type: null });
        return true;
      } else {
        console.error('Müşteri güncelleme hatası:', response.status);
        return false;
      }
    } catch (err) {
      console.error('Müşteri güncelleme API hatası:', err);
      return false;
    }
  };

  const toggleMusteriBlacklist = async (musteriId: number, neden?: string) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Musteriler/${musteriId}/toggle-blacklist`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(neden)
      });

      if (response.ok) {
        await fetchMusteriler();
        return true;
      }
      return false;
    } catch (err) {
      console.error('Blacklist toggle hatası:', err);
      return false;
    }
  };

  const deleteMuesteri = async (musteriId: number) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Musteriler/${musteriId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        await fetchMusteriler();
        return true;
      } else {
        const errorData = await response.json();
        alert(errorData.message || 'Müşteri silinemedi');
        return false;
      }
    } catch (err) {
      console.error('Müşteri silme hatası:', err);
      return false;
    }
  };

  useEffect(() => {
    if (isAdmin) {
      fetchAdminData();
      fetchSystemReport();
    } else if (isAcente) {
      fetchAcenteData();
    } else {
      fetchKullaniciData();
    }
  }, [isAdmin, isAcente]);

  // Müşteri sekmesi için data loading
  useEffect(() => {
    if (activeTab === 'customers' && (isAdmin || isAcente)) {
      fetchLookupData();
      fetchMusteriler();
      fetchMusteriIstatistikleri();
    }
  }, [activeTab, isAdmin, isAcente]);

  // Müşteri arama parametreleri değiştiğinde
  useEffect(() => {
    if (activeTab === 'customers') {
      fetchMusteriler();
    }
  }, [musteriSearchParams]);

  const fetchKullaniciData = async () => {
    try {
      setLoading(true);
      // Kullanıcı için profil ve poliçe bilgileri
      const response = await fetch('http://localhost:5000/api/Kullanicilar/profile', {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      if (response.ok) {
        const data = await response.json();
        // TODO: Kullanıcı verileri işle
      }
    } catch (err) {
      setError('Kullanıcı verileri yüklenemedi');
    } finally {
      setLoading(false);
    }
  };

  const fetchAcenteData = async () => {
    try {
      setLoading(true);
      // Acente için satış ve komisyon verileri
      // TODO: Acente dashboard API'leri
    } catch (err) {
      setError('Acente verileri yüklenemedi');
    } finally {
      setLoading(false);
    }
  };

  const fetchAdminData = async () => {
    try {
      setLoading(true);
      
      // Admin kullanıcı listesi
      const usersResponse = await fetch('http://localhost:5000/api/Admin/users', {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      if (usersResponse.ok) {
        const users = await usersResponse.json();
        setAdminUsers(users);
      } else {
        throw new Error(`API Hatası: ${usersResponse.status}`);
      }

      // Dashboard istatistikleri
      const statsResponse = await fetch('http://localhost:5000/api/Admin/dashboard-stats', {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      if (statsResponse.ok) {
        const stats = await statsResponse.json();
        setDashboardStats(stats);
      }

    } catch (err) {
      setError(err instanceof Error ? err.message : 'Admin verileri yüklenemedi. Lütfen tekrar deneyin.');
      console.error('Admin data fetch error:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchSystemReport = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/Admin/reports/system-overview', {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      if (response.ok) {
        const report = await response.json();
        setSystemReport(report);
      } else {
        console.error('System report fetch failed:', response.status);
      }
    } catch (err) {
      console.error('System report fetch error:', err);
    }
  };

  const fetchUserPerformance = async (userId: string) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Admin/users/${userId}/performance`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      if (response.ok) {
        const performance = await response.json();
        setUserPerformance(performance);
      } else {
        console.error('User performance fetch failed:', response.status);
      }
    } catch (err) {
      console.error('User performance fetch error:', err);
    }
  };

  const fetchUserAudit = async (userId: string) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Admin/users/${userId}/audit`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      if (response.ok) {
        const audit = await response.json();
        setUserAudit(audit);
      } else {
        console.error('User audit fetch failed:', response.status);
      }
    } catch (err) {
      console.error('User audit fetch error:', err);
    }
  };

  const handleCreateUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch('http://localhost:5000/api/Admin/users', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(createUserForm)
      });

      if (response.ok) {
        alert('Kullanıcı başarıyla oluşturuldu!');
        setCreateUserForm({
          ad: '',
          soyad: '',
          email: '',
          password: '',
          telefon: '',
          role: 'KULLANICI',
          pozisyon: '',
          departman: '',
          yoneticiId: ''
        });
        fetchAdminData();
        setActiveTab('users');
      } else {
        const error = await response.json();
        alert(`Hata: ${error.message}`);
      }
    } catch (err) {
      alert('Kullanıcı oluşturulurken hata oluştu.');
    }
  };

  const toggleUserLock = async (userId: string, isLocked: boolean) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Admin/users/${userId}/lock`, {
        method: 'PATCH',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(!isLocked)
      });

      if (response.ok) {
        const result = await response.json();
        alert(result.message);
        fetchAdminData();
      } else {
        alert('İşlem başarısız oldu.');
      }
    } catch (err) {
      alert('Bir hata oluştu.');
    }
  };

  const toggleUserActive = async (userId: string) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Admin/users/${userId}/toggle-active`, {
        method: 'PATCH',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const result = await response.json();
        alert(result.message);
        fetchAdminData();
      } else {
        alert('İşlem başarısız oldu.');
      }
    } catch (err) {
      alert('Bir hata oluştu.');
    }
  };

  const handleBulkAction = async (action: string) => {
    if (bulkSelection.length === 0) {
      alert('Lütfen en az bir kullanıcı seçin.');
      return;
    }

    try {
      const response = await fetch('http://localhost:5000/api/Admin/users/bulk-action', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          userIds: bulkSelection,
          action: action
        })
      });

      if (response.ok) {
        const result = await response.json();
        alert(`${result.successCount} kullanıcı başarıyla işlendi.`);
        setBulkSelection([]);
        fetchAdminData();
      } else {
        alert('Bulk işlem başarısız oldu.');
      }
    } catch (err) {
      alert('Bir hata oluştu.');
    }
  };

  const resetUserPassword = async (userId: string, newPassword: string) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Admin/users/${userId}/reset-password`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ newPassword })
      });

      if (response.ok) {
        alert('Şifre başarıyla sıfırlandı.');
      } else {
        alert('Şifre sıfırlanamadı.');
      }
    } catch (err) {
      alert('Bir hata oluştu.');
    }
  };

  const addUserNote = async (userId: string, note: string) => {
    try {
      const response = await fetch(`http://localhost:5000/api/Admin/users/${userId}/notes`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ note })
      });

      if (response.ok) {
        alert('Not başarıyla eklendi.');
        fetchAdminData();
      } else {
        alert('Not eklenemedi.');
      }
    } catch (err) {
      alert('Bir hata oluştu.');
    }
  };

  const openUserModal = (type: 'view' | 'edit' | 'performance' | 'audit' | 'security', user: AdminUser) => {
    setSelectedUser(user);
    setUserModal({type, user});
    
    if (type === 'performance') {
      fetchUserPerformance(user.id);
    } else if (type === 'audit') {
      fetchUserAudit(user.id);
    }
  };

  const filteredUsers = adminUsers.filter(user => {
    const matchesSearch = !filters.search || 
      user.ad.toLowerCase().includes(filters.search.toLowerCase()) ||
      user.soyad.toLowerCase().includes(filters.search.toLowerCase()) ||
      user.email.toLowerCase().includes(filters.search.toLowerCase());
    
    const matchesRole = !filters.role || user.roller.includes(filters.role);
    
    const matchesStatus = !filters.status || 
      (filters.status === 'active' && user.aktifMi && !user.hesapKilitliMi) ||
      (filters.status === 'inactive' && (!user.aktifMi || user.hesapKilitliMi));
    
    const matchesDepartment = !filters.department || user.departman === filters.department;
    
    return matchesSearch && matchesRole && matchesStatus && matchesDepartment;
  });

  const handleLogout = () => {
    logout();
  };

  if (loading) {
    return (
      <div className="dashboard loading-state">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <h2>🔄 Sistem Verileri Yükleniyor...</h2>
          <p>Lütfen bekleyin, veriler yükleniyor...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="dashboard error-state">
        <header className="dashboard-header">
          <div className="header-top">
            <h1>🔧 Admin Paneli - Sigorta Yönetim Sistemi</h1>
            <div className="user-info">
              <span>Admin: <strong>{user?.ad} {user?.soyad}</strong></span>
              <button onClick={handleLogout} className="logout-btn">Çıkış Yap</button>
            </div>
          </div>
        </header>
        <div className="error-content">
          <div className="error-box">
            <h2>⚠️ Bağlantı Hatası</h2>
            <p style={{ color: 'red' }}>{error}</p>
            <div className="error-actions">
              <button onClick={fetchAdminData} className="retry-btn">
                🔄 Tekrar Dene
              </button>
              <button onClick={() => window.location.reload()} className="refresh-btn">
                🔃 Sayfayı Yenile
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // ADMIN DASHBOARD
  if (isAdmin) {
    return (
      <div className="dashboard admin-dashboard">
        <header className="dashboard-header">
          <div className="header-top">
            <h1>🔧 Admin Paneli - Sigorta Yönetim Sistemi</h1>
            <div className="user-info">
              <span>Admin: <strong>{user?.ad} {user?.soyad}</strong></span>
              <button onClick={handleLogout} className="logout-btn">Çıkış Yap</button>
            </div>
          </div>
          
          <nav className="admin-nav">
            <button 
              className={activeTab === 'overview' ? 'active' : ''}
              onClick={() => setActiveTab('overview')}
            >
              📊 Genel Bakış
            </button>
            <button 
              className={activeTab === 'users' ? 'active' : ''}
              onClick={() => setActiveTab('users')}
            >
              👥 Kullanıcı Yönetimi
            </button>
            <button 
              className={activeTab === 'create' ? 'active' : ''}
              onClick={() => setActiveTab('create')}
            >
              ➕ Yeni Kullanıcı
            </button>
            <button 
              className={activeTab === 'reports' ? 'active' : ''}
              onClick={() => setActiveTab('reports')}
            >
              📈 Raporlar
            </button>
            <button 
              className={activeTab === 'security' ? 'active' : ''}
              onClick={() => setActiveTab('security')}
            >
              🔒 Güvenlik
            </button>
            <button 
              className={activeTab === 'hierarchy' ? 'active' : ''}
              onClick={() => setActiveTab('hierarchy')}
            >
              🏢 Organizasyon
            </button>
            <button 
              className={activeTab === 'customers' ? 'active' : ''}
              onClick={() => setActiveTab('customers')}
            >
              👤 Müşteri Yönetimi
            </button>
          </nav>
        </header>

        <main className="dashboard-main">
          {activeTab === 'overview' && dashboardStats && (
            <div className="content-section">
              <h2>🏠 Sistem İstatistikleri</h2>
              
              <div className="stats-grid">
                <div className="stat-card primary">
                  <div className="stat-icon">👥</div>
                  <div className="stat-info">
                    <h3>Toplam Kullanıcı</h3>
                    <div className="stat-number">{dashboardStats.totalUsers}</div>
                    <small>Sistemde kayıtlı</small>
                  </div>
                </div>
                <div className="stat-card success">
                  <div className="stat-icon">✅</div>
                  <div className="stat-info">
                    <h3>Aktif Kullanıcılar</h3>
                    <div className="stat-number">{dashboardStats.activeUsers}</div>
                    <small>Giriş yapabilir</small>
                  </div>
                </div>
                <div className="stat-card warning">
                  <div className="stat-icon">🔒</div>
                  <div className="stat-info">
                    <h3>Kilitli Hesaplar</h3>
                    <div className="stat-number">{dashboardStats.lockedUsers}</div>
                    <small>Güvenlik nedeniyle</small>
                  </div>
                </div>
                <div className="stat-card info">
                  <div className="stat-icon">📧</div>
                  <div className="stat-info">
                    <h3>Doğrulanmış E-posta</h3>
                    <div className="stat-number">{dashboardStats.verifiedUsers}</div>
                    <small>E-posta doğrulandı</small>
                  </div>
                </div>
              </div>

              {systemReport && (
                <div className="system-metrics">
                  <div className="metrics-row">
                    <div className="metric-card">
                      <h4>🔐 Güvenlik Metrikleri</h4>
                      <div className="metric-items">
                        <div className="metric-item">
                          <span>Başarısız Giriş Denemeleri</span>
                          <strong>{systemReport?.securityMetrics?.failedLoginAttempts || 0}</strong>
                        </div>
                        <div className="metric-item">
                          <span>Bu Hafta Kilitlenen Hesaplar</span>
                          <strong>{systemReport?.securityMetrics?.accountsLockedThisWeek || 0}</strong>
                        </div>
                        <div className="metric-item">
                          <span>Bu Ay Şifre Sıfırlama</span>
                          <strong>{systemReport?.securityMetrics?.passwordResetsThisMonth || 0}</strong>
                        </div>
                      </div>
                    </div>

                    <div className="metric-card">
                      <h4>📈 Aktivite Metrikleri</h4>
                      <div className="metric-items">
                        <div className="metric-item">
                          <span>Toplam Giriş</span>
                          <strong>{systemReport?.systemActivity?.totalLogins || 0}</strong>
                        </div>
                        <div className="metric-item">
                          <span>Bu Hafta Giriş</span>
                          <strong>{systemReport?.systemActivity?.loginsThisWeek || 0}</strong>
                        </div>
                        <div className="metric-item">
                          <span>Ortalama Oturum (dk)</span>
                          <strong>{systemReport?.systemActivity?.averageSessionDuration ? Math.round(systemReport.systemActivity.averageSessionDuration) : 0}</strong>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              )}

              <div className="dashboard-sections">
                <div className="section">
                  <h3>👨‍💼 Rol Dağılımı</h3>
                  <div className="role-distribution">
                    {dashboardStats.roleDistribution.map((role, index) => (
                      <div key={index} className="role-item">
                        <span className={`role-badge ${role.role.toLowerCase()}`}>
                          {role.role}
                        </span>
                        <span className="role-count">{role.count}</span>
                        <div className="role-progress">
                          <div 
                            className="progress-bar" 
                            style={{width: `${(role.count / dashboardStats.totalUsers) * 100}%`}}
                          ></div>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>

                <div className="section">
                  <h3>🕐 Son Girişler</h3>
                  <div className="recent-logins">
                    {dashboardStats.recentLogins.length > 0 ? dashboardStats.recentLogins.map((login, index) => (
                      <div key={index} className="login-item">
                        <div className="user-avatar small">
                          {login.ad.charAt(0)}{login.soyad.charAt(0)}
                        </div>
                        <div className="login-info">
                          <span className="user-name">{login.ad} {login.soyad}</span>
                          <span className="login-date">
                            {new Date(login.sonGirisTarihi).toLocaleString('tr-TR')}
                          </span>
                        </div>
                      </div>
                    )) : <p>Henüz giriş kaydı yok.</p>}
                  </div>
                </div>
              </div>
            </div>
          )}

          {activeTab === 'users' && (
            <div className="content-section">
              <div className="section-header">
                <h2>👥 Gelişmiş Kullanıcı Yönetimi</h2>
                <div className="header-actions">
                  <button 
                    className="btn btn-primary"
                    onClick={() => setActiveTab('create')}
                  >
                    ➕ Yeni Kullanıcı
                  </button>
                </div>
              </div>
              
              {/* Filtreleme ve Arama */}
              <div className="user-filters">
                <div className="filter-row">
                  <div className="filter-group">
                    <input
                      type="text"
                      placeholder="🔍 Ad, soyad, e-posta ile ara..."
                      className="search-input"
                      value={filters.search}
                      onChange={(e) => setFilters({...filters, search: e.target.value})}
                    />
                  </div>
                  <div className="filter-group">
                    <select 
                      className="filter-select"
                      value={filters.role}
                      onChange={(e) => setFilters({...filters, role: e.target.value})}
                    >
                      <option value="">👨‍💼 Tüm Roller</option>
                      <option value="ADMIN">Admin</option>
                      <option value="ACENTE">Acente</option>
                      <option value="KULLANICI">Kullanıcı</option>
                    </select>
                  </div>
                  <div className="filter-group">
                    <select 
                      className="filter-select"
                      value={filters.status}
                      onChange={(e) => setFilters({...filters, status: e.target.value})}
                    >
                      <option value="">🚦 Tüm Durumlar</option>
                      <option value="active">Aktif</option>
                      <option value="inactive">Pasif/Kilitli</option>
                    </select>
                  </div>
                  <div className="filter-group">
                    <select 
                      className="filter-select"
                      value={filters.department}
                      onChange={(e) => setFilters({...filters, department: e.target.value})}
                    >
                      <option value="">🏢 Tüm Departmanlar</option>
                      <option value="IT">IT</option>
                      <option value="Satış">Satış</option>
                      <option value="Muhasebe">Muhasebe</option>
                      <option value="İnsan Kaynakları">İnsan Kaynakları</option>
                    </select>
                  </div>
                </div>
              </div>

              <div className="users-table-container">
                <div className="table-info">
                  <span>{filteredUsers.length} kullanıcı görüntüleniyor</span>
                  {bulkSelection.length > 0 && (
                    <span className="selection-info">
                      {bulkSelection.length} kullanıcı seçili
                    </span>
                  )}
                </div>

                <table className="users-table">
                  <thead>
                    <tr>
                      <th>
                        <input 
                          type="checkbox" 
                          onChange={(e) => {
                            if (e.target.checked) {
                              setBulkSelection(filteredUsers.map(u => u.id));
                            } else {
                              setBulkSelection([]);
                            }
                          }}
                          checked={bulkSelection.length === filteredUsers.length && filteredUsers.length > 0}
                        />
                      </th>
                      <th>👤 Kullanıcı</th>
                      <th>📧 İletişim</th>
                      <th>👨‍💼 Roller</th>
                      <th>🏢 Organizasyon</th>
                      <th>🚦 Durum</th>
                      <th>📊 Performans</th>
                      <th>🕐 Son Aktivite</th>
                      <th>⚙️ İşlemler</th>
                    </tr>
                  </thead>
                  <tbody>
                    {Array.isArray(filteredUsers) && filteredUsers.length > 0 ? filteredUsers.map((user) => (
                      <tr key={user.id} className={!user.aktifMi || user.hesapKilitliMi ? 'inactive-user' : ''}>
                        <td>
                          <input 
                            type="checkbox" 
                            checked={bulkSelection.includes(user.id)}
                            onChange={(e) => {
                              if (e.target.checked) {
                                setBulkSelection([...bulkSelection, user.id]);
                              } else {
                                setBulkSelection(bulkSelection.filter(id => id !== user.id));
                              }
                            }}
                          />
                        </td>
                        <td>
                          <div className="user-info">
                            <div className="user-avatar">
                              {user.ad.charAt(0)}{user.soyad.charAt(0)}
                            </div>
                            <div className="user-details">
                              <strong>{user.ad} {user.soyad}</strong>
                              <small>ID: {user.id.substring(0, 8)}...</small>
                              {user.yoneticiAdi && (
                                <small className="manager-info">👔 Yönetici: {user.yoneticiAdi}</small>
                              )}
                            </div>
                          </div>
                        </td>
                        <td>
                          <div className="contact-info">
                            <div className="email">
                              📧 {user.email}
                              {user.emailDogrulandi && <span className="verified">✓</span>}
                            </div>
                            <small className="phone">
                              📱 {user.telefon || 'Telefon yok'}
                              {user.telefonDogrulandi && <span className="verified">✓</span>}
                            </small>
                          </div>
                        </td>
                        <td>
                          <div className="roles">
                            {user.roller.map(role => (
                              <span key={role} className={`role-badge ${role.toLowerCase()}`}>
                                {role}
                              </span>
                            ))}
                          </div>
                        </td>
                        <td>
                          <div className="org-info">
                            <div className="position">
                              💼 {user.pozisyon || 'Belirtilmemiş'}
                            </div>
                            <small className="department">
                              🏢 {user.departman || 'Departman yok'}
                            </small>
                          </div>
                        </td>
                        <td>
                          <div className="status-info">
                            <span className={`status ${!user.aktifMi || user.hesapKilitliMi ? 'inactive' : 'active'}`}>
                              {!user.aktifMi ? '⏸️ Pasif' : 
                               user.hesapKilitliMi ? '🔒 Kilitli' : 
                               '✅ Aktif'}
                            </span>
                            <div className="status-details">
                              {user.emailDogrulandi && <span className="verified">✅ E-posta</span>}
                              {user.telefonDogrulandi && <span className="verified">✅ Telefon</span>}
                              {user.basarisizGirisSayisi > 0 && (
                                <span className="warning">⚠️ {user.basarisizGirisSayisi} başarısız giriş</span>
                              )}
                            </div>
                          </div>
                        </td>
                        <td>
                          <div className="performance-preview">
                            <button
                              className="performance-btn"
                              onClick={() => openUserModal('performance', user)}
                              title="Performans Detayları"
                            >
                              📊 Görüntüle
                            </button>
                          </div>
                        </td>
                        <td>
                          <div className="activity-info">
                            <div className="last-login">
                              {user.sonGirisTarihi ? 
                                new Date(user.sonGirisTarihi).toLocaleDateString('tr-TR') : 
                                'Hiç giriş yapmamış'
                              }
                            </div>
                            <small className="ip-info">
                              {user.sonIpAdresi ? `🌐 ${user.sonIpAdresi}` : ''}
                            </small>
                          </div>
                        </td>
                        <td>
                          <div className="action-buttons">
                            <button
                              className="action-btn view"
                              title="Detayları Görüntüle"
                              onClick={() => openUserModal('view', user)}
                            >
                              👁️
                            </button>
                            <button
                              className="action-btn edit"
                              title="Düzenle"
                              onClick={() => openUserModal('edit', user)}
                            >
                              ✏️
                            </button>
                            <button
                              className="action-btn audit"
                              title="Audit Log"
                              onClick={() => openUserModal('audit', user)}
                            >
                              📋
                            </button>
                            <button
                              className="action-btn security"
                              title="Güvenlik Ayarları"
                              onClick={() => openUserModal('security', user)}
                            >
                              🔒
                            </button>
                            <button
                              onClick={() => toggleUserLock(user.id, !!user.hesapKilitliMi)}
                              className={`action-btn ${user.hesapKilitliMi ? 'unlock' : 'lock'}`}
                              title={user.hesapKilitliMi ? 'Kilidi Aç' : 'Kilitle'}
                            >
                              {user.hesapKilitliMi ? '🔓' : '🔒'}
                            </button>
                            <button
                              onClick={() => toggleUserActive(user.id)}
                              className={`action-btn ${user.aktifMi ? 'deactivate' : 'activate'}`}
                              title={user.aktifMi ? 'Pasifleştir' : 'Aktifleştir'}
                            >
                              {user.aktifMi ? '⏸️' : '▶️'}
                            </button>
                          </div>
                        </td>
                      </tr>
                    )) : (
                      <tr>
                        <td colSpan={9} className="no-data">
                          {adminUsers.length === 0 ? (
                            <div className="no-users">
                              <h3>❌ Kullanıcı Bulunamadı</h3>
                              <p>Henüz sistemde kullanıcı bulunmuyor veya API bağlantısında sorun var.</p>
                              <button onClick={fetchAdminData} className="retry-btn">
                                🔄 Tekrar Dene
                              </button>
                            </div>
                          ) : (
                            <div className="no-results">
                              <h3>🔍 Filtreye Uygun Kullanıcı Yok</h3>
                              <p>Arama kriterlerinizi değiştirip tekrar deneyin.</p>
                              <button onClick={() => setFilters({search: '', role: '', status: '', department: '', position: ''})} className="clear-filters-btn">
                                🗑️ Filtreleri Temizle
                              </button>
                            </div>
                          )
                        }
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>

              {/* Bulk Actions */}
              {bulkSelection.length > 0 && (
                <div className="bulk-actions">
                  <div className="bulk-left">
                    <span className="selection-count">
                      {bulkSelection.length} kullanıcı seçildi
                    </span>
                  </div>
                  <div className="bulk-right">
                    <button 
                      className="bulk-btn activate"
                      onClick={() => handleBulkAction('activate')}
                    >
                      ✅ Aktifleştir
                    </button>
                    <button 
                      className="bulk-btn deactivate"
                      onClick={() => handleBulkAction('deactivate')}
                    >
                      ⏸️ Pasifleştir
                    </button>
                    <button 
                      className="bulk-btn lock"
                      onClick={() => handleBulkAction('lock')}
                    >
                      🔒 Kilitle
                    </button>
                    <button 
                      className="bulk-btn unlock"
                      onClick={() => handleBulkAction('unlock')}
                    >
                      🔓 Kilidi Aç
                    </button>
                  </div>
                </div>
              )}
            </div>
          )}

          {activeTab === 'create' && (
            <div className="content-section">
              <h2>➕ Yeni Kullanıcı Oluştur</h2>
              
              <form onSubmit={handleCreateUser} className="create-user-form">
                <div className="form-grid">
                  <div className="form-group">
                    <label>👤 Ad:</label>
                    <input
                      type="text"
                      value={createUserForm.ad}
                      onChange={(e) => setCreateUserForm({...createUserForm, ad: e.target.value})}
                      required
                      placeholder="Adı girin"
                    />
                  </div>
                  
                  <div className="form-group">
                    <label>👤 Soyad:</label>
                    <input
                      type="text"
                      value={createUserForm.soyad}
                      onChange={(e) => setCreateUserForm({...createUserForm, soyad: e.target.value})}
                      required
                      placeholder="Soyadı girin"
                    />
                  </div>
                  
                  <div className="form-group">
                    <label>📧 E-posta:</label>
                    <input
                      type="email"
                      value={createUserForm.email}
                      onChange={(e) => setCreateUserForm({...createUserForm, email: e.target.value})}
                      required
                      placeholder="email@domain.com"
                    />
                  </div>
                  
                  <div className="form-group">
                    <label>🔒 Şifre:</label>
                    <input
                      type="password"
                      value={createUserForm.password}
                      onChange={(e) => setCreateUserForm({...createUserForm, password: e.target.value})}
                      required
                      minLength={6}
                      placeholder="En az 6 karakter"
                    />
                  </div>
                  
                  <div className="form-group">
                    <label>📱 Telefon:</label>
                    <input
                      type="tel"
                      value={createUserForm.telefon}
                      onChange={(e) => setCreateUserForm({...createUserForm, telefon: e.target.value})}
                      placeholder="+90 555 123 4567"
                    />
                  </div>
                  
                  <div className="form-group">
                    <label>👨‍💼 Rol:</label>
                    <select
                      value={createUserForm.role}
                      onChange={(e) => setCreateUserForm({...createUserForm, role: e.target.value})}
                    >
                      <option value="KULLANICI">Kullanıcı</option>
                      <option value="ACENTE">Acente</option>
                      <option value="ADMIN">Admin</option>
                    </select>
                  </div>

                  <div className="form-group">
                    <label>💼 Pozisyon:</label>
                    <input
                      type="text"
                      value={createUserForm.pozisyon}
                      onChange={(e) => setCreateUserForm({...createUserForm, pozisyon: e.target.value})}
                      placeholder="Ör: Sigorta Uzmanı"
                    />
                  </div>

                  <div className="form-group">
                    <label>🏢 Departman:</label>
                    <select
                      value={createUserForm.departman}
                      onChange={(e) => setCreateUserForm({...createUserForm, departman: e.target.value})}
                    >
                      <option value="">Departman Seçin</option>
                      <option value="IT">IT</option>
                      <option value="Satış">Satış</option>
                      <option value="Muhasebe">Muhasebe</option>
                      <option value="İnsan Kaynakları">İnsan Kaynakları</option>
                      <option value="Operasyon">Operasyon</option>
                      <option value="Hasar">Hasar</option>
                    </select>
                  </div>
                </div>
                
                <div className="form-actions">
                  <button type="submit" className="submit-btn">
                    ➕ Kullanıcı Oluştur
                  </button>
                  <button 
                    type="button" 
                    className="cancel-btn"
                    onClick={() => setActiveTab('users')}
                  >
                    ❌ İptal
                  </button>
                </div>
              </form>
            </div>
          )}

          {activeTab === 'reports' && systemReport && (
            <div className="content-section">
              <h2>📈 Sistem Raporları</h2>
              
              <div className="reports-grid">
                <div className="report-card">
                  <h3>👥 Kullanıcı Metrikleri</h3>
                  <div className="report-content">
                    <div className="metric-row">
                      <span>Toplam Kullanıcı:</span>
                      <strong>{systemReport?.userMetrics?.totalUsers || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Aktif Kullanıcılar:</span>
                      <strong>{systemReport?.userMetrics?.activeUsers || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Bu Ay Yeni Kayıtlar:</span>
                      <strong>{systemReport?.userMetrics?.newUsersThisMonth || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Kilitli Hesaplar:</span>
                      <strong>{systemReport?.userMetrics?.lockedAccounts || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Doğrulanmamış E-postalar:</span>
                      <strong>{systemReport?.userMetrics?.unverifiedEmails || 0}</strong>
                    </div>
                  </div>
                </div>

                <div className="report-card">
                  <h3>🏢 Departman Dağılımı</h3>
                  <div className="report-content">
                    {systemReport?.departmentBreakdown?.map((dept, index) => (
                      <div key={index} className="metric-row">
                        <span>{dept.department}:</span>
                        <strong>{dept.count}</strong>
                      </div>
                    )) || <p>Departman verisi yükleniyor...</p>}
                  </div>
                </div>

                <div className="report-card">
                  <h3>🔐 Güvenlik Raporu</h3>
                  <div className="report-content">
                    <div className="metric-row">
                      <span>Başarısız Giriş Denemeleri:</span>
                      <strong>{systemReport?.securityMetrics?.failedLoginAttempts || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Bu Hafta Kilitlenen:</span>
                      <strong>{systemReport?.securityMetrics?.accountsLockedThisWeek || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Bu Ay Şifre Sıfırlama:</span>
                      <strong>{systemReport?.securityMetrics?.passwordResetsThisMonth || 0}</strong>
                    </div>
                  </div>
                </div>

                <div className="report-card">
                  <h3>📊 Aktivite Raporu</h3>
                  <div className="report-content">
                    <div className="metric-row">
                      <span>Toplam Giriş:</span>
                      <strong>{systemReport?.systemActivity?.totalLogins || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Bu Hafta Giriş:</span>
                      <strong>{systemReport?.systemActivity?.loginsThisWeek || 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Ortalama Oturum (dk):</span>
                      <strong>{systemReport?.systemActivity?.averageSessionDuration ? Math.round(systemReport.systemActivity.averageSessionDuration) : 0}</strong>
                    </div>
                    <div className="metric-row">
                      <span>Sistem Logları:</span>
                      <strong>{systemReport?.systemActivity?.systemLogs || 0}</strong>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {activeTab === 'security' && (
            <div className="content-section">
              <h2>🔒 Güvenlik Yönetimi</h2>
              
              <div className="security-sections">
                <div className="security-card">
                  <h3>🛡️ Sistem Güvenlik Durumu</h3>
                  <div className="security-status">
                    <div className="status-item good">
                      <span className="status-icon">✅</span>
                      <span>HTTPS Aktif</span>
                    </div>
                    <div className="status-item good">
                      <span className="status-icon">✅</span>
                      <span>JWT Token Güvenliği</span>
                    </div>
                    <div className="status-item warning">
                      <span className="status-icon">⚠️</span>
                      <span>2FA Devre Dışı</span>
                    </div>
                    <div className="status-item good">
                      <span className="status-icon">✅</span>
                      <span>Şifre Politikası Aktif</span>
                    </div>
                  </div>
                </div>

                <div className="security-card">
                  <h3>🚨 Güvenlik Alertleri</h3>
                  <div className="security-alerts">
                    {systemReport && systemReport.securityMetrics && systemReport.securityMetrics.failedLoginAttempts > 10 && (
                      <div className="alert warning">
                        <span className="alert-icon">⚠️</span>
                        <div className="alert-content">
                          <strong>Yüksek Başarısız Giriş Denemesi</strong>
                          <p>{systemReport.securityMetrics.failedLoginAttempts} başarısız giriş denemesi tespit edildi.</p>
                        </div>
                      </div>
                    )}
                    {systemReport && systemReport.securityMetrics && systemReport.securityMetrics.accountsLockedThisWeek > 5 && (
                      <div className="alert danger">
                        <span className="alert-icon">🚨</span>
                        <div className="alert-content">
                          <strong>Anormal Hesap Kilitleme</strong>
                          <p>Bu hafta {systemReport.securityMetrics.accountsLockedThisWeek} hesap kilitlendi.</p>
                        </div>
                      </div>
                    )}
                    <div className="alert info">
                      <span className="alert-icon">ℹ️</span>
                      <div className="alert-content">
                        <strong>Sistem Normal</strong>
                        <p>Güvenlik metrikleri normal aralıkta.</p>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="security-card">
                  <h3>⚙️ Güvenlik Ayarları</h3>
                  <div className="security-settings">
                    <div className="setting-item">
                      <label>Oturum Zaman Aşımı (dk):</label>
                      <input type="number" defaultValue="30" min="5" max="120" />
                    </div>
                    <div className="setting-item">
                      <label>Maksimum Başarısız Giriş:</label>
                      <input type="number" defaultValue="5" min="3" max="10" />
                    </div>
                    <div className="setting-item">
                      <label>Şifre Minimum Uzunluk:</label>
                      <input type="number" defaultValue="6" min="6" max="20" />
                    </div>
                    <div className="setting-item">
                      <label>
                        <input type="checkbox" />
                        Güçlü Şifre Zorunlu
                      </label>
                    </div>
                    <div className="setting-item">
                      <label>
                        <input type="checkbox" />
                        2FA Zorunlu (Adminler)
                      </label>
                    </div>
                  </div>
                  <button className="save-settings-btn">💾 Ayarları Kaydet</button>
                </div>
              </div>
            </div>
          )}

          {activeTab === 'hierarchy' && (
            <div className="content-section">
              <h2>🏢 Organizasyon Hiyerarşisi</h2>
              
              <div className="hierarchy-view">
                <div className="hierarchy-card">
                  <h3>👥 Departman Bazlı Görünüm</h3>
                  {systemReport && systemReport.departmentBreakdown && systemReport.departmentBreakdown.map((dept, index) => (
                    <div key={index} className="department-section">
                      <div className="department-header">
                        <h4>{dept.department}</h4>
                        <span className="dept-count">{dept.count} kişi</span>
                      </div>
                      <div className="department-users">
                        {adminUsers
                          .filter(user => (user.departman || 'Tanımsız') === dept.department)
                          .map(user => (
                            <div key={user.id} className="hierarchy-user">
                              <div className="user-avatar small">
                                {user.ad.charAt(0)}{user.soyad.charAt(0)}
                              </div>
                              <div className="user-info">
                                <strong>{user.ad} {user.soyad}</strong>
                                <small>{user.pozisyon || 'Pozisyon belirtilmemiş'}</small>
                                <div className="user-roles">
                                  {user.roller.map(role => (
                                    <span key={role} className={`role-badge small ${role.toLowerCase()}`}>
                                      {role}
                                    </span>
                                  ))}
                                </div>
                              </div>
                            </div>
                          ))}
                      </div>
                    </div>
                  ))}
                </div>

                <div className="hierarchy-card">
                  <h3>🎯 Yönetim Hiyerarşisi</h3>
                  <div className="management-tree">
                    {adminUsers
                      .filter(user => !user.yoneticiId)
                      .map(manager => (
                        <div key={manager.id} className="manager-branch">
                          <div className="manager-node">
                            <div className="user-avatar">
                              {manager.ad.charAt(0)}{manager.soyad.charAt(0)}
                            </div>
                            <div className="manager-info">
                              <strong>{manager.ad} {manager.soyad}</strong>
                              <small>{manager.pozisyon}</small>
                              <div className="manager-roles">
                                {manager.roller.map(role => (
                                  <span key={role} className={`role-badge small ${role.toLowerCase()}`}>
                                    {role}
                                  </span>
                                ))}
                              </div>
                            </div>
                          </div>
                          <div className="subordinates">
                            {adminUsers
                              .filter(user => user.yoneticiId === manager.id)
                              .map(subordinate => (
                                <div key={subordinate.id} className="subordinate-node">
                                  <div className="user-avatar small">
                                    {subordinate.ad.charAt(0)}{subordinate.soyad.charAt(0)}
                                  </div>
                                  <div className="subordinate-info">
                                    <strong>{subordinate.ad} {subordinate.soyad}</strong>
                                    <small>{subordinate.pozisyon}</small>
                                  </div>
                                </div>
                              ))}
                          </div>
                        </div>
                      ))}
                  </div>
                </div>
              </div>
            </div>
          )}
        </main>

        {/* Kullanıcı Modal'ları */}
        {userModal.type && userModal.user && (
          <UserModal 
            type={userModal.type}
            user={userModal.user}
            performance={userPerformance}
            audit={userAudit}
            onClose={() => {
              setUserModal({type: null});
              setUserPerformance(null);
              setUserAudit(null);
            }}
            onSave={(updatedUser: AdminUser) => {
              // TODO: Kullanıcı güncelleme API'si
              fetchAdminData();
              setUserModal({type: null});
            }}
            onPasswordReset={resetUserPassword}
            onAddNote={addUserNote}
          />
        )}

        {/* MÜŞTERİ YÖNETİMİ SEKMESİ */}
        {activeTab === 'customers' && (isAdmin || isAcente) && (
          <div className="content-section">
            <h2>👤 Müşteri Yönetimi</h2>
            
            {/* İstatistikler */}
            {musteriIstatistikleri && (
              <div className="stats-grid">
                <div className="stat-card primary">
                  <div className="stat-icon">👥</div>
                  <div className="stat-info">
                    <h3>Toplam Müşteri</h3>
                    <div className="stat-number">{musteriIstatistikleri.toplam_musteri_sayisi}</div>
                    <small>Kayıtlı müşteri sayısı</small>
                  </div>
                </div>
                <div className="stat-card success">
                  <div className="stat-icon">👤</div>
                  <div className="stat-info">
                    <h3>Bireysel</h3>
                    <div className="stat-number">{musteriIstatistikleri.bireysel_musteri_sayisi}</div>
                    <small>Bireysel müşteriler</small>
                  </div>
                </div>
                <div className="stat-card info">
                  <div className="stat-icon">🏢</div>
                  <div className="stat-info">
                    <h3>Kurumsal</h3>
                    <div className="stat-number">{musteriIstatistikleri.kurumsal_musteri_sayisi}</div>
                    <small>Kurumsal müşteriler</small>
                  </div>
                </div>
                <div className="stat-card warning">
                  <div className="stat-icon">🚫</div>
                  <div className="stat-info">
                    <h3>Blacklist</h3>
                    <div className="stat-number">{musteriIstatistikleri.blacklist_musteri_sayisi}</div>
                    <small>Kara listedeki müşteriler</small>
                  </div>
                </div>
              </div>
            )}

            {/* Arama ve Filtreler */}
            <div className="filters-section">
              <div className="search-box">
                <input
                  type="text"
                  placeholder="Müşteri ara (ad, soyad, şirket, e-posta, telefon, TC, vergi no...)"
                  value={musteriSearchParams.arama_metni || ''}
                  onChange={(e) => setMusteriSearchParams(prev => ({ ...prev, arama_metni: e.target.value }))}
                />
                <button onClick={() => fetchMusteriler()}>🔍 Ara</button>
              </div>
              
              <div className="filter-row">
                <select
                  value={musteriSearchParams.tip_id || ''}
                  onChange={(e) => setMusteriSearchParams(prev => ({ ...prev, tip_id: e.target.value ? parseInt(e.target.value) : undefined }))}
                >
                  <option value="">Tüm Tipler</option>
                  {lookupData?.musteri_tipleri.map(tip => (
                    <option key={tip.id} value={tip.id}>{tip.text}</option>
                  ))}
                </select>

                <select
                  value={musteriSearchParams.blacklist_mi?.toString() || ''}
                  onChange={(e) => setMusteriSearchParams(prev => ({ 
                    ...prev, 
                    blacklist_mi: e.target.value === '' ? undefined : e.target.value === 'true' 
                  }))}
                >
                  <option value="">Tüm Durumlar</option>
                  <option value="false">Aktif</option>
                  <option value="true">Blacklist</option>
                </select>

                <button 
                  className="btn btn-primary"
                  onClick={() => setMusteriModal({ type: 'create' })}
                >
                  ➕ Yeni Müşteri
                </button>
              </div>
            </div>

            {/* Müşteri Listesi */}
            <div className="table-container">
              {musteriLoading ? (
                <div className="loading">Müşteriler yükleniyor...</div>
              ) : (
                <>
                  <table className="data-table">
                    <thead>
                      <tr>
                        <th>Müşteri No</th>
                        <th>Ad/Şirket</th>
                        <th>Tip</th>
                        <th>E-posta</th>
                        <th>Telefon</th>
                        <th>Şehir</th>
                        <th>Durum</th>
                        <th>Kayıt Tarihi</th>
                        <th>İşlemler</th>
                      </tr>
                    </thead>
                    <tbody>
                      {Array.isArray(musteriler) && musteriler.length > 0 ? musteriler.map(musteri => (
                        <tr key={musteri.id}>
                          <td>
                            <strong>{musteri.musteri_no}</strong>
                          </td>
                          <td>
                            <div className="customer-info">
                              <strong>{musteri.tam_ad}</strong>
                              {musteri.sirket_adi && <div className="company-name">{musteri.sirket_adi}</div>}
                            </div>
                          </td>
                          <td>
                            <span className={`badge ${musteri.tip_adi.includes('Bireysel') ? 'info' : 'success'}`}>
                              {musteri.tip_adi}
                            </span>
                          </td>
                          <td>{musteri.eposta || 'Belirtilmemiş'}</td>
                          <td>{musteri.telefon || 'Belirtilmemiş'}</td>
                          <td>{musteri.adres_il || 'Belirtilmemiş'}</td>
                          <td>
                            <span className={`status-badge ${musteri.blacklist_mi ? 'inactive' : 'active'}`}>
                              {musteri.blacklist_mi ? '🚫 Blacklist' : '✅ Aktif'}
                            </span>
                          </td>
                          <td>{new Date(musteri.kayit_tarihi).toLocaleDateString('tr-TR')}</td>
                          <td>
                            <div className="action-buttons">
                              <button 
                                className="btn btn-small btn-info"
                                onClick={async () => {
                                  const detay = await fetchMusteriDetay(musteri.id);
                                  if (detay) {
                                    setMusteriModal({ type: 'view', musteri: detay });
                                  }
                                }}
                              >
                                👁️ Görüntüle
                              </button>
                              <button 
                                className="btn btn-small btn-primary"
                                onClick={async () => {
                                  const detay = await fetchMusteriDetay(musteri.id);
                                  if (detay) {
                                    setMusteriModal({ type: 'edit', musteri: detay });
                                  }
                                }}
                              >
                                ✏️ Düzenle
                              </button>
                              <button 
                                className={`btn btn-small ${musteri.blacklist_mi ? 'btn-success' : 'btn-warning'}`}
                                onClick={() => {
                                  const neden = musteri.blacklist_mi ? '' : prompt('Blacklist nedeni:');
                                  if (musteri.blacklist_mi || neden !== null) {
                                    toggleMusteriBlacklist(musteri.id, neden || undefined);
                                  }
                                }}
                              >
                                {musteri.blacklist_mi ? '✅ Aktif Et' : '🚫 Blacklist'}
                              </button>
                              {isAdmin && (
                                <button 
                                  className="btn btn-small btn-danger"
                                  onClick={() => {
                                    if (window.confirm('Bu müşteriyi silmek istediğinizden emin misiniz?')) {
                                      deleteMuesteri(musteri.id);
                                    }
                                  }}
                                >
                                  🗑️ Sil
                                </button>
                              )}
                            </div>
                          </td>
                        </tr>
                      )) : (
                        <tr><td colSpan={9}>Kayıt bulunamadı</td></tr>
                      )}
                    </tbody>
                  </table>

                  {/* Sayfalama */}
                  {musteriTotalCount > musteriSearchParams.sayfa_boyutu && (
                    <div className="pagination">
                      <span>
                        Toplam {musteriTotalCount} müşteri, 
                        Sayfa {musteriSearchParams.sayfa} / {Math.ceil(musteriTotalCount / musteriSearchParams.sayfa_boyutu)}
                      </span>
                      <div className="pagination-buttons">
                        <button 
                          disabled={musteriSearchParams.sayfa <= 1}
                          onClick={() => setMusteriSearchParams(prev => ({ ...prev, sayfa: prev.sayfa - 1 }))}
                        >
                          ⬅️ Önceki
                        </button>
                        <button 
                          disabled={musteriSearchParams.sayfa >= Math.ceil(musteriTotalCount / musteriSearchParams.sayfa_boyutu)}
                          onClick={() => setMusteriSearchParams(prev => ({ ...prev, sayfa: prev.sayfa + 1 }))}
                        >
                          Sonraki ➡️
                        </button>
                      </div>
                    </div>
                  )}
                </>
              )}
            </div>
          </div>
        )}

        {/* Müşteri Modal'ları */}
        {musteriModal.type && (
          <div className="modal-overlay" onClick={() => setMusteriModal({ type: null })}>
            <div className="modal-content large" onClick={(e) => e.stopPropagation()}>
              <div className="modal-header">
                <h2>
                  {musteriModal.type === 'create' && '➕ Yeni Müşteri Oluştur'}
                  {musteriModal.type === 'edit' && '✏️ Müşteri Düzenle'}
                  {musteriModal.type === 'view' && '👁️ Müşteri Detayları'}
                </h2>
                <button className="modal-close" onClick={() => setMusteriModal({ type: null })}>❌</button>
              </div>

              <div className="modal-body">
                {musteriModal.type === 'view' && musteriModal.musteri && (
                  <div className="customer-details">
                    <div className="details-grid">
                      <div className="detail-section">
                        <h3>👤 Temel Bilgiler</h3>
                        <div className="detail-row">
                          <label>Müşteri No:</label>
                          <span>{musteriModal.musteri.musteri_no}</span>
                        </div>
                        <div className="detail-row">
                          <label>Tip:</label>
                          <span>{musteriModal.musteri.tip_adi}</span>
                        </div>
                        {musteriModal.musteri.sirket_adi ? (
                          <>
                            <div className="detail-row">
                              <label>Şirket Adı:</label>
                              <span>{musteriModal.musteri.sirket_adi}</span>
                            </div>
                            <div className="detail-row">
                              <label>Vergi No:</label>
                              <span>{musteriModal.musteri.vergi_no}</span>
                            </div>
                          </>
                        ) : (
                          <>
                            <div className="detail-row">
                              <label>Ad Soyad:</label>
                              <span>{musteriModal.musteri.ad} {musteriModal.musteri.soyad}</span>
                            </div>
                            <div className="detail-row">
                              <label>TC Kimlik No:</label>
                              <span>{musteriModal.musteri.tc_kimlik_no}</span>
                            </div>
                          </>
                        )}
                      </div>

                      <div className="detail-section">
                        <h3>📞 İletişim Bilgileri</h3>
                        <div className="detail-row">
                          <label>E-posta:</label>
                          <span>{musteriModal.musteri.eposta || 'Belirtilmemiş'}</span>
                        </div>
                        <div className="detail-row">
                          <label>Telefon:</label>
                          <span>{musteriModal.musteri.telefon || 'Belirtilmemiş'}</span>
                        </div>
                        <div className="detail-row">
                          <label>Cep Telefonu:</label>
                          <span>{musteriModal.musteri.cep_telefonu || 'Belirtilmemiş'}</span>
                        </div>
                      </div>

                      <div className="detail-section">
                        <h3>🏠 Adres Bilgileri</h3>
                        <div className="detail-row">
                          <label>İl:</label>
                          <span>{musteriModal.musteri.adres_il || 'Belirtilmemiş'}</span>
                        </div>
                        <div className="detail-row">
                          <label>İlçe:</label>
                          <span>{musteriModal.musteri.adres_ilce || 'Belirtilmemiş'}</span>
                        </div>
                        <div className="detail-row">
                          <label>Adres:</label>
                          <span>{musteriModal.musteri.adres_detay || 'Belirtilmemiş'}</span>
                        </div>
                      </div>

                      <div className="detail-section">
                        <h3>📊 İstatistikler</h3>
                        <div className="stats-mini-grid">
                          <div className="mini-stat">
                            <span className="mini-stat-number">{musteriModal.musteri.toplam_police_sayisi}</span>
                            <span className="mini-stat-label">Toplam Poliçe</span>
                          </div>
                          <div className="mini-stat">
                            <span className="mini-stat-number">{musteriModal.musteri.aktif_police_sayisi}</span>
                            <span className="mini-stat-label">Aktif Poliçe</span>
                          </div>
                          <div className="mini-stat">
                            <span className="mini-stat-number">{musteriModal.musteri.hasar_sayisi}</span>
                            <span className="mini-stat-label">Hasar Dosyası</span>
                          </div>
                          <div className="mini-stat">
                            <span className="mini-stat-number">₺{musteriModal.musteri.toplam_prim_tutari.toLocaleString('tr-TR')}</span>
                            <span className="mini-stat-label">Toplam Prim</span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                )}

                {(musteriModal.type === 'create' || musteriModal.type === 'edit') && (
                  <form onSubmit={async (e) => {
                    e.preventDefault();
                    const formData = new FormData(e.target as HTMLFormElement);
                    const musteriData: MusteriCreateDto = {
                      tip_id: parseInt(formData.get('tip_id') as string),
                      ad: formData.get('ad') as string || undefined,
                      soyad: formData.get('soyad') as string || undefined,
                      sirket_adi: formData.get('sirket_adi') as string || undefined,
                      vergi_no: formData.get('vergi_no') as string || undefined,
                      tc_kimlik_no: formData.get('tc_kimlik_no') as string || undefined,
                      eposta: formData.get('eposta') as string || undefined,
                      telefon: formData.get('telefon') as string || undefined,
                      cep_telefonu: formData.get('cep_telefonu') as string || undefined,
                      dogum_tarihi: formData.get('dogum_tarihi') as string || undefined,
                      cinsiyet_id: formData.get('cinsiyet_id') ? parseInt(formData.get('cinsiyet_id') as string) : undefined,
                      medeni_durum_id: formData.get('medeni_durum_id') ? parseInt(formData.get('medeni_durum_id') as string) : undefined,
                      meslek: formData.get('meslek') as string || undefined,
                      egitim_durumu_id: formData.get('egitim_durumu_id') ? parseInt(formData.get('egitim_durumu_id') as string) : undefined,
                      aylik_gelir: formData.get('aylik_gelir') ? parseFloat(formData.get('aylik_gelir') as string) : undefined,
                      adres_il: formData.get('adres_il') as string || undefined,
                      adres_ilce: formData.get('adres_ilce') as string || undefined,
                      adres_mahalle: formData.get('adres_mahalle') as string || undefined,
                      adres_detay: formData.get('adres_detay') as string || undefined,
                      posta_kodu: formData.get('posta_kodu') as string || undefined,
                      not_bilgileri: formData.get('not_bilgileri') as string || undefined,
                      blacklist_mi: formData.get('blacklist_mi') === 'on',
                      blacklist_nedeni: formData.get('blacklist_nedeni') as string || undefined,
                    };

                    const success = musteriModal.type === 'create' 
                      ? await createMusteri(musteriData)
                      : await updateMusteri({ ...musteriData, id: musteriModal.musteri!.id });

                    if (success) {
                      alert(musteriModal.type === 'create' ? 'Müşteri başarıyla oluşturuldu' : 'Müşteri başarıyla güncellendi');
                    } else {
                      alert('İşlem başarısız oldu');
                    }
                  }}>
                    <div className="form-grid">
                      <div className="form-group">
                        <label>Müşteri Tipi *</label>
                        <select name="tip_id" required defaultValue={musteriModal.musteri?.tip_id || ''}>
                          <option value="">Seçiniz</option>
                          {lookupData?.musteri_tipleri.map(tip => (
                            <option key={tip.id} value={tip.id}>{tip.text}</option>
                          ))}
                        </select>
                      </div>

                      <div className="form-group">
                        <label>Ad</label>
                        <input type="text" name="ad" defaultValue={musteriModal.musteri?.ad || ''} />
                      </div>

                      <div className="form-group">
                        <label>Soyad</label>
                        <input type="text" name="soyad" defaultValue={musteriModal.musteri?.soyad || ''} />
                      </div>

                      <div className="form-group">
                        <label>Şirket Adı</label>
                        <input type="text" name="sirket_adi" defaultValue={musteriModal.musteri?.sirket_adi || ''} />
                      </div>

                      <div className="form-group">
                        <label>TC Kimlik No</label>
                        <input type="text" name="tc_kimlik_no" maxLength={11} defaultValue={musteriModal.musteri?.tc_kimlik_no || ''} />
                      </div>

                      <div className="form-group">
                        <label>Vergi No</label>
                        <input type="text" name="vergi_no" defaultValue={musteriModal.musteri?.vergi_no || ''} />
                      </div>

                      <div className="form-group">
                        <label>E-posta</label>
                        <input type="email" name="eposta" defaultValue={musteriModal.musteri?.eposta || ''} />
                      </div>

                      <div className="form-group">
                        <label>Telefon</label>
                        <input type="tel" name="telefon" defaultValue={musteriModal.musteri?.telefon || ''} />
                      </div>

                      <div className="form-group">
                        <label>Cep Telefonu</label>
                        <input type="tel" name="cep_telefonu" defaultValue={musteriModal.musteri?.cep_telefonu || ''} />
                      </div>

                      <div className="form-group">
                        <label>Doğum Tarihi</label>
                        <input type="date" name="dogum_tarihi" defaultValue={musteriModal.musteri?.dogum_tarihi || ''} />
                      </div>

                      <div className="form-group">
                        <label>Cinsiyet</label>
                        <select name="cinsiyet_id" defaultValue={musteriModal.musteri?.cinsiyet_id || ''}>
                          <option value="">Seçiniz</option>
                          {lookupData?.cinsiyetler.map(cinsiyet => (
                            <option key={cinsiyet.id} value={cinsiyet.id}>{cinsiyet.text}</option>
                          ))}
                        </select>
                      </div>

                      <div className="form-group">
                        <label>Medeni Durum</label>
                        <select name="medeni_durum_id" defaultValue={musteriModal.musteri?.medeni_durum_id || ''}>
                          <option value="">Seçiniz</option>
                          {lookupData?.medeni_durumlar.map(durum => (
                            <option key={durum.id} value={durum.id}>{durum.text}</option>
                          ))}
                        </select>
                      </div>

                      <div className="form-group">
                        <label>Meslek</label>
                        <input type="text" name="meslek" defaultValue={musteriModal.musteri?.meslek || ''} />
                      </div>

                      <div className="form-group">
                        <label>Eğitim Durumu</label>
                        <select name="egitim_durumu_id" defaultValue={musteriModal.musteri?.egitim_durumu_id || ''}>
                          <option value="">Seçiniz</option>
                          {lookupData?.egitim_durumlari.map(egitim => (
                            <option key={egitim.id} value={egitim.id}>{egitim.text}</option>
                          ))}
                        </select>
                      </div>

                      <div className="form-group">
                        <label>Aylık Gelir</label>
                        <input type="number" name="aylik_gelir" step="0.01" defaultValue={musteriModal.musteri?.aylik_gelir || ''} />
                      </div>

                      <div className="form-group">
                        <label>İl</label>
                        <input type="text" name="adres_il" defaultValue={musteriModal.musteri?.adres_il || ''} />
                      </div>

                      <div className="form-group">
                        <label>İlçe</label>
                        <input type="text" name="adres_ilce" defaultValue={musteriModal.musteri?.adres_ilce || ''} />
                      </div>

                      <div className="form-group">
                        <label>Mahalle</label>
                        <input type="text" name="adres_mahalle" defaultValue={musteriModal.musteri?.adres_mahalle || ''} />
                      </div>

                      <div className="form-group full-width">
                        <label>Adres Detayı</label>
                        <textarea name="adres_detay" rows={3} defaultValue={musteriModal.musteri?.adres_detay || ''} />
                      </div>

                      <div className="form-group">
                        <label>Posta Kodu</label>
                        <input type="text" name="posta_kodu" defaultValue={musteriModal.musteri?.posta_kodu || ''} />
                      </div>

                      <div className="form-group full-width">
                        <label>Not Bilgileri</label>
                        <textarea name="not_bilgileri" rows={3} defaultValue={musteriModal.musteri?.not_bilgileri || ''} />
                      </div>

                      <div className="form-group">
                        <label>
                          <input 
                            type="checkbox" 
                            name="blacklist_mi" 
                            defaultChecked={musteriModal.musteri?.blacklist_mi || false} 
                          />
                          Blacklist
                        </label>
                      </div>

                      <div className="form-group">
                        <label>Blacklist Nedeni</label>
                        <input type="text" name="blacklist_nedeni" defaultValue={musteriModal.musteri?.blacklist_nedeni || ''} />
                      </div>
                    </div>

                    <div className="modal-footer">
                      <button type="submit" className="btn btn-primary">
                        💾 {musteriModal.type === 'create' ? 'Oluştur' : 'Güncelle'}
                      </button>
                      <button type="button" className="btn btn-secondary" onClick={() => setMusteriModal({ type: null })}>
                        ❌ İptal
                      </button>
                    </div>
                  </form>
                )}
              </div>
            </div>
          </div>
        )}
      </div>
    );
  }

  // ACENTE DASHBOARD
  if (isAcente) {
    return (
      <div className="dashboard acente-dashboard">
        <header className="dashboard-header">
          <div className="header-top">
            <h1>🏢 Acente Paneli - Satış ve Komisyon Yönetimi</h1>
            <div className="user-info">
              <span>Acente: <strong>{user?.ad} {user?.soyad}</strong></span>
              <button onClick={handleLogout} className="logout-btn">Çıkış Yap</button>
            </div>
          </div>
          
          <nav className="admin-nav acente-nav">
            <button 
              className={activeTab === 'overview' ? 'active' : ''}
              onClick={() => setActiveTab('overview')}
            >
              📊 Genel Bakış
            </button>
            <button 
              className={activeTab === 'sales' ? 'active' : ''}
              onClick={() => setActiveTab('sales')}
            >
              💼 Satışlarım
            </button>
            <button 
              className={activeTab === 'commission' ? 'active' : ''}
              onClick={() => setActiveTab('commission')}
            >
              💰 Komisyon
            </button>
            <button 
              className={activeTab === 'policies' ? 'active' : ''}
              onClick={() => setActiveTab('policies')}
            >
              📋 Poliçeler
            </button>
          </nav>
        </header>

        <main className="dashboard-main">
          {activeTab === 'overview' && (
            <div className="content-section">
              <h2>Acente Performans Özeti</h2>
              
              <div className="stats-grid">
                <div className="stat-card acente-stat">
                  <h3>Bu Ay Satış</h3>
                  <div className="stat-number">0</div>
                  <small>Poliçe</small>
                </div>
                <div className="stat-card acente-stat">
                  <h3>Toplam Prim</h3>
                  <div className="stat-number">₺0</div>
                  <small>Bu ay</small>
                </div>
                <div className="stat-card acente-stat">
                  <h3>Komisyon</h3>
                  <div className="stat-number">₺0</div>
                  <small>Bu ay</small>
                </div>
                <div className="stat-card acente-stat">
                  <h3>Aktif Müşteri</h3>
                  <div className="stat-number">0</div>
                  <small>Toplam</small>
                </div>
              </div>

              <div className="dashboard-sections">
                <div className="section">
                  <h3>Aylık Satış Trendi</h3>
                  <div className="chart-placeholder">
                    <p>📈 Grafik yakında eklenecek</p>
                    <p>Son 6 ay satış performansı burada görüntülenecek</p>
                  </div>
                </div>

                <div className="section">
                  <h3>Yaklaşan Yenilemeler</h3>
                  <div className="renewal-list">
                    <p>📅 Yenilenmesi gereken poliçeler burada listelenecek</p>
                  </div>
                </div>
              </div>
            </div>
          )}

          {activeTab === 'sales' && (
            <div className="content-section">
              <h2>Satış Geçmişi ve Teklifler</h2>
              <div className="empty-state">
                <h3>💼 Satış Modülü</h3>
                <p>Satış geçmişiniz ve aktif teklifleriniz burada görüntülenecek.</p>
                <p><strong>Yakında eklenecek özellikler:</strong></p>
                <ul>
                  <li>Yeni teklif oluşturma</li>
                  <li>Mevcut teklifleri yönetme</li>
                  <li>Satış geçmişi</li>
                  <li>Müşteri takibi</li>
                </ul>
              </div>
            </div>
          )}

          {activeTab === 'commission' && (
            <div className="content-section">
              <h2>Komisyon Hesapları</h2>
              <div className="empty-state">
                <h3>💰 Komisyon Modülü</h3>
                <p>Komisyon hesaplarınız ve ödemeleriniz burada görüntülenecek.</p>
                <p><strong>Yakında eklenecek özellikler:</strong></p>
                <ul>
                  <li>Aylık komisyon raporu</li>
                  <li>Ödeme geçmişi</li>
                  <li>Komisyon hesaplama</li>
                  <li>Performans bonusları</li>
                </ul>
              </div>
            </div>
          )}

          {activeTab === 'policies' && (
            <div className="content-section">
              <h2>Poliçe Yönetimi</h2>
              <div className="empty-state">
                <h3>📋 Poliçe Modülü</h3>
                <p>Sattığınız poliçeler ve durumları burada görüntülenecek.</p>
                <p><strong>Yakında eklenecek özellikler:</strong></p>
                <ul>
                  <li>Aktif poliçeler</li>
                  <li>Poliçe detayları</li>
                  <li>Yenileme takibi</li>
                  <li>İptal işlemleri</li>
                </ul>
              </div>
            </div>
          )}
        </main>
      </div>
    );
  }

  // KULLANICI DASHBOARD
  return (
    <div className="dashboard kullanici-dashboard">
      <header className="dashboard-header">
        <div className="header-top">
          <h1>👤 Müşteri Paneli - Poliçe ve Hasar Takibi</h1>
          <div className="user-info">
            <span>Hoş geldiniz, <strong>{user?.ad} {user?.soyad}</strong></span>
            <span className="user-role">({user?.roles?.join(', ')})</span>
            {user?.emailDogrulandi && (
              <span className="verified-badge">✅ Doğrulanmış</span>
            )}
            <button onClick={handleLogout} className="logout-btn">Çıkış Yap</button>
          </div>
        </div>
        
        <nav className="admin-nav kullanici-nav">
          <button 
            className={activeTab === 'overview' ? 'active' : ''}
            onClick={() => setActiveTab('overview')}
          >
            🏠 Ana Sayfa
          </button>
          <button 
            className={activeTab === 'policies' ? 'active' : ''}
            onClick={() => setActiveTab('policies')}
          >
            📋 Poliçelerim
          </button>
          <button 
            className={activeTab === 'claims' ? 'active' : ''}
            onClick={() => setActiveTab('claims')}
          >
            ⚡ Hasarlarım
          </button>
        </nav>
      </header>

      <main className="dashboard-main">
        {activeTab === 'overview' && (
          <div className="content-section">
            <h2>Hesap Özetim</h2>
            
            <div className="stats-grid">
              <div className="stat-card kullanici-stat">
                <h3>Aktif Poliçe</h3>
                <div className="stat-number">0</div>
                <small>Adet</small>
              </div>
              <div className="stat-card kullanici-stat">
                <h3>Bekleyen Ödeme</h3>
                <div className="stat-number">₺0</div>
                <small>Toplam</small>
              </div>
              <div className="stat-card kullanici-stat">
                <h3>Açık Hasar</h3>
                <div className="stat-number">0</div>
                <small>Adet</small>
              </div>
              <div className="stat-card kullanici-stat">
                <h3>Puan</h3>
                <div className="stat-number">100</div>
                <small>Sadakat puanı</small>
              </div>
            </div>

            <div className="dashboard-sections">
              <div className="section">
                <h3>Profil Bilgileri</h3>
                <div className="profile-summary">
                  <p><strong>Ad Soyad:</strong> {user?.ad} {user?.soyad}</p>
                  <p><strong>E-posta:</strong> {user?.email}</p>
                  <p><strong>Telefon:</strong> {user?.telefon}</p>
                  <p><strong>Kayıt Tarihi:</strong> {user?.kayitTarihi ? new Date(user.kayitTarihi).toLocaleDateString('tr-TR') : '-'}</p>
                  <p><strong>E-posta Durumu:</strong> {user?.emailDogrulandi ? '✅ Doğrulanmış' : '❌ Doğrulanmamış'}</p>
                </div>
              </div>

              <div className="section">
                <h3>Son Aktiviteler</h3>
                <div className="activity-list">
                  <p>📝 Henüz aktivite kaydı bulunmuyor.</p>
                  <p>Poliçe satın aldığınızda veya hasar bildirimi yaptığınızda aktiviteler burada görünecek.</p>
                </div>
              </div>
            </div>
          </div>
        )}

        {activeTab === 'policies' && (
          <div className="content-section">
            <h2>Poliçelerim</h2>
            <div className="empty-state">
              <h3>📋 Poliçe Modülü</h3>
              <p>Sahip olduğunuz poliçeler burada görüntülenecek.</p>
              <p><strong>Yakında eklenecek özellikler:</strong></p>
              <ul>
                <li>Aktif poliçe listesi</li>
                <li>Poliçe detayları</li>
                <li>Ödeme geçmişi</li>
                <li>Yenileme işlemleri</li>
                <li>Poliçe dökümanları</li>
              </ul>
            </div>
          </div>
        )}

        {activeTab === 'claims' && (
          <div className="content-section">
            <h2>Hasar Takibi</h2>
            <div className="empty-state">
              <h3>⚡ Hasar Modülü</h3>
              <p>Hasar bildirimleri ve takip işlemleri burada görüntülenecek.</p>
              <p><strong>Yakında eklenecek özellikler:</strong></p>
              <ul>
                <li>Yeni hasar bildirimi</li>
                <li>Hasar durumu takibi</li>
                <li>Ekspertiz raporları</li>
                <li>Ödeme bilgileri</li>
                <li>Döküman yükleme</li>
              </ul>
            </div>
          </div>
        )}
      </main>
    </div>
  );
};

export default Dashboard; 