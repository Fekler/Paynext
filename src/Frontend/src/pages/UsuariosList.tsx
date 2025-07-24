import React, { useEffect, useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, CircularProgress, Chip, IconButton, Tooltip, Button, Snackbar, Alert as MuiAlert } from '@mui/material';
import { Add, Edit, Delete } from '@mui/icons-material';
import api from '../api';
import UserFormModal from './UserFormModal';

interface Usuario {
  uuid: string;
  fullName: string;
  email: string;
  userRole: number;
  isActive: boolean;
  phone: string;
  document: string;
  clientId: string;
}

const UsuariosList: React.FC = () => {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editUser, setEditUser] = useState<Usuario | null>(null);
  const [snackbar, setSnackbar] = useState<{open: boolean, message: string, severity: 'success'|'error'}>({open: false, message: '', severity: 'success'});

  const fetchUsuarios = async () => {
    setLoading(true);
    try {
      const response = await api.get('/api/v1/Users');
      setUsuarios(response.data.data || response.data);
    } catch (err: any) {
      setError('Erro ao buscar usuários');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsuarios();
  }, []);

  const handleAdd = () => {
    setEditUser(null);
    setModalOpen(true);
  };

  const handleEdit = (user: Usuario) => {
    setEditUser(user);
    setModalOpen(true);
  };

  const handleDelete = async (uuid: string) => {
    if (!window.confirm('Tem certeza que deseja excluir este usuário?')) return;
    try {
      await api.delete(`/api/v1/Users/${uuid}`);
      setSnackbar({open: true, message: 'Usuário excluído com sucesso!', severity: 'success'});
      fetchUsuarios();
    } catch {
      setSnackbar({open: true, message: 'Erro ao excluir usuário', severity: 'error'});
    }
  };

  const handleModalSubmit = async (data: any) => {
    try {
      if (editUser) {
        await api.put('/api/v1/Users', { ...editUser, ...data, uuid: editUser.uuid });
        setSnackbar({open: true, message: 'Usuário atualizado com sucesso!', severity: 'success'});
      } else {
        await api.post('/api/v1/Users', data);
        setSnackbar({open: true, message: 'Usuário adicionado com sucesso!', severity: 'success'});
      }
      setModalOpen(false);
      fetchUsuarios();
    } catch {
      setSnackbar({open: true, message: 'Erro ao salvar usuário', severity: 'error'});
    }
  };

  if (loading) return <div className="flex justify-center mt-10"><CircularProgress /></div>;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <div className="min-h-screen w-screen flex flex-col items-center bg-gray-100" style={{ color: '#111' }}>
      <div className="flex flex-col md:flex-row justify-between items-center w-full max-w-6xl mt-8 mb-4 gap-4 px-2 md:px-8">
        <Typography variant="h6" color='black'>Usuários</Typography>
        <Button variant="contained" color="primary" startIcon={<Add />} onClick={handleAdd}>
          Adicionar Usuário
        </Button>
      </div>
      <TableContainer
        component={Paper}
        className="w-full max-w-6xl overflow-x-auto"
        style={{ marginBottom: 32 }}
      >
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Nome</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Perfil</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Telefone</TableCell>
              <TableCell>Documento</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {usuarios.map((usuario) => (
              <TableRow key={usuario.uuid}>
                <TableCell>{usuario.fullName}</TableCell>
                <TableCell>{usuario.email}</TableCell>
                <TableCell>{usuario.userRole === 1 ? 'Admin' : 'Cliente'}</TableCell>
                <TableCell>
                  <Chip label={usuario.isActive ? 'Ativo' : 'Inativo'} color={usuario.isActive ? 'success' : 'default'} size="small" />
                </TableCell>
                <TableCell>{usuario.phone}</TableCell>
                <TableCell>{usuario.document}</TableCell>
                <TableCell>
                  <Tooltip title="Editar"><IconButton onClick={() => handleEdit(usuario)}><Edit /></IconButton></Tooltip>
                  <Tooltip title="Excluir"><IconButton onClick={() => handleDelete(usuario.uuid)}><Delete /></IconButton></Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <UserFormModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onSubmit={handleModalSubmit}
        initialData={editUser}
        isEdit={!!editUser}
      />
      <Snackbar open={snackbar.open} autoHideDuration={3000} onClose={() => setSnackbar({...snackbar, open: false})}>
        <MuiAlert onClose={() => setSnackbar({...snackbar, open: false})} severity={snackbar.severity} sx={{ width: '100%' }}>
          {snackbar.message}
        </MuiAlert>
      </Snackbar>
    </div>
  );
};

export default UsuariosList; 