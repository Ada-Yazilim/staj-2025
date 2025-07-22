import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import './Login.css';

interface RegisterProps {
  onSwitchToLogin: () => void;
}

const Register: React.FC<RegisterProps> = ({ onSwitchToLogin }) => {
  const [formData, setFormData] = useState({
    ad: '',
    soyad: '',
    email: '',
    password: '',
    confirmPassword: '',
    telefon: ''
  });
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const { register } = useAuth();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setMessage('');

    // Form validasyonu
    if (formData.password !== formData.confirmPassword) {
      setMessage('Şifreler eşleşmiyor');
      setLoading(false);
      return;
    }

    if (formData.password.length < 6) {
      setMessage('Şifre en az 6 karakter olmalıdır');
      setLoading(false);
      return;
    }

    const result = await register(formData);
    
    if (!result.success) {
      setMessage(result.message);
    }
    
    setLoading(false);
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>Sigorta Yönetim Platformu</h2>
        <h3>Kayıt Ol</h3>
        
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="ad">Ad:</label>
            <input
              type="text"
              id="ad"
              name="ad"
              value={formData.ad}
              onChange={handleChange}
              required
              disabled={loading}
              placeholder="Adınızı giriniz"
            />
          </div>

          <div className="form-group">
            <label htmlFor="soyad">Soyad:</label>
            <input
              type="text"
              id="soyad"
              name="soyad"
              value={formData.soyad}
              onChange={handleChange}
              required
              disabled={loading}
              placeholder="Soyadınızı giriniz"
            />
          </div>

          <div className="form-group">
            <label htmlFor="email">E-posta:</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
              disabled={loading}
              placeholder="E-posta adresinizi giriniz"
            />
          </div>

          <div className="form-group">
            <label htmlFor="telefon">Telefon (İsteğe bağlı):</label>
            <input
              type="tel"
              id="telefon"
              name="telefon"
              value={formData.telefon}
              onChange={handleChange}
              disabled={loading}
              placeholder="Telefon numaranızı giriniz"
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Şifre:</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
              disabled={loading}
              placeholder="Şifrenizi giriniz (en az 6 karakter)"
            />
          </div>

          <div className="form-group">
            <label htmlFor="confirmPassword">Şifre Tekrarı:</label>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
              disabled={loading}
              placeholder="Şifrenizi tekrar giriniz"
            />
          </div>

          {message && (
            <div className={`message ${message.includes('başarı') ? 'success' : 'error'}`}>
              {message}
            </div>
          )}

          <button 
            type="submit" 
            disabled={loading}
            className="login-button"
          >
            {loading ? 'Kayıt yapılıyor...' : 'Kayıt Ol'}
          </button>
        </form>

        <div className="auth-switch">
          <p>Zaten hesabınız var mı?</p>
          <button 
            type="button" 
            onClick={onSwitchToLogin}
            className="switch-button"
          >
            Giriş Yap
          </button>
        </div>
      </div>
    </div>
  );
};

export default Register; 