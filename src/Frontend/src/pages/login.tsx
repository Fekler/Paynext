import React, { useState } from 'react';
import { TextField, Button, Typography, Alert } from '@mui/material';
import { useDispatch } from 'react-redux';
import api from '../api';
import { loginSuccess } from '../authSlice';

const LoginPage: React.FC = () => {
  const dispatch = useDispatch();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const response = await api.post('auth-login', { email, password });
      dispatch(loginSuccess({ token: response.data.token, user: response.data.user }));
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao autenticar');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="w-screen h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-10 rounded-lg shadow-lg w-full max-w-md">
        <Typography variant="h5" className="mb-8 text-center font-bold">Login</Typography>
        <form onSubmit={handleLogin} className="flex flex-col gap-6">
          <TextField
            label="Email"
            type="email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            required
            fullWidth
          />
          <TextField
            label="Senha"
            type="password"
            value={password}
            onChange={e => setPassword(e.target.value)}
            required
            fullWidth
          />
          {error && <Alert severity="error">{error}</Alert>}
          <Button
            type="submit"
            variant="contained"
            color="primary"
            disabled={loading}
            fullWidth
            size="large"
            style={{ fontWeight: 700 }}
          >
            {loading ? 'Entrando...' : 'Entrar'}
          </Button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
