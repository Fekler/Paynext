import React, { useEffect, useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, CircularProgress } from '@mui/material';
import api from '../api';

interface Usuario {
  id: string;
  email: string;
  role: string;
}

const UsuariosList: React.FC = () => {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchUsuarios = async () => {
      try {
        // Ajuste o endpoint conforme sua API
        const response = await api.get('/api/users');
        setUsuarios(response.data.data || response.data);
      } catch (err: any) {
        setError('Erro ao buscar usuários');
      } finally {
        setLoading(false);
      }
    };
    fetchUsuarios();
  }, []);

  if (loading) return <div className="flex justify-center mt-10"><CircularProgress /></div>;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <TableContainer component={Paper} className="mt-8">
      <Typography variant="h6" className="p-4">Usuários</Typography>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Email</TableCell>
            <TableCell>Perfil</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {usuarios.map((usuario) => (
            <TableRow key={usuario.id}>
              <TableCell>{usuario.email}</TableCell>
              <TableCell>{usuario.role}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default UsuariosList; 