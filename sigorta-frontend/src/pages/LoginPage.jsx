import { useState } from 'react';

function LoginPage() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();
        const response = await fetch('http://localhost:8080/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        const data = await response.text(); // token geliyor
        if (response.ok) {
            localStorage.setItem('token', data);
            alert("Giriş başarılı!");
        } else {
            alert("Giriş başarısız: " + data);
        }
    };

    return (
        <form onSubmit={handleLogin}>
            <h2>Giriş Yap</h2>
            <input
                type="text"
                placeholder="Kullanıcı Adı"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="Şifre"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button type="submit">Giriş</button>
        </form>
    );
}

export default LoginPage;
