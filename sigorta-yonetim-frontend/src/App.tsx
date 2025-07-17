import React, { useState, useEffect } from 'react';
import './App.css';

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

function App() {
  const [kullanicilar, setKullanicilar] = useState<Kullanici[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchKullanicilar();
  }, []);

  const fetchKullanicilar = async () => {
    try {
      setLoading(true);
      const response = await fetch('http://localhost:5293/api/Kullanicilar');
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      setKullanicilar(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="App">
        <header className="App-header">
          <h1>Sigorta Yönetim Platformu</h1>
          <p>Veriler yükleniyor...</p>
        </header>
      </div>
    );
  }

  if (error) {
    return (
      <div className="App">
        <header className="App-header">
          <h1>Sigorta Yönetim Platformu</h1>
          <p style={{ color: 'red' }}>Hata: {error}</p>
          <button onClick={fetchKullanicilar}>Tekrar Dene</button>
        </header>
      </div>
    );
  }

  return (
    <div className="App">
      <header className="App-header">
        <h1>Sigorta Yönetim Platformu</h1>
        <h2>Kullanıcı Listesi</h2>
        
        <div className="kullanici-listesi">
          {kullanicilar.map((kullanici) => (
            <div key={kullanici.id} className="kullanici-karti">
              <h3>{kullanici.ad} {kullanici.soyad}</h3>
              <p><strong>E-posta:</strong> {kullanici.eposta}</p>
              <p><strong>Telefon:</strong> {kullanici.telefon}</p>
              <p><strong>Durum:</strong> {kullanici.durum_id === 1 ? 'Aktif' : 'Pasif'}</p>
              <p><strong>E-posta Doğrulandı:</strong> {kullanici.email_dogrulandi ? 'Evet' : 'Hayır'}</p>
              <p><strong>Telefon Doğrulandı:</strong> {kullanici.telefon_dogrulandi ? 'Evet' : 'Hayır'}</p>
              <p><strong>Kayıt Tarihi:</strong> {new Date(kullanici.kayit_tarihi).toLocaleDateString('tr-TR')}</p>
            </div>
          ))}
        </div>
        
        <button onClick={fetchKullanicilar} className="yenile-btn">
          Listeyi Yenile
        </button>
      </header>
    </div>
  );
}

export default App;
