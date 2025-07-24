import React, { useState, useEffect } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, MenuItem } from '@mui/material';

interface UserFormModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: any) => void;
  initialData?: any;
  isEdit?: boolean;
}

const UserFormModal: React.FC<UserFormModalProps> = ({ open, onClose, onSubmit, initialData, isEdit }) => {
  const [form, setForm] = useState({
    fullName: '',
    email: '',
    phone: '',
    password: '',
    document: '',
    //clientId: '',
    userRole: 0,
  });

  useEffect(() => {
    if (initialData) {
      setForm({ ...initialData, password: '' }); // nunca preenche senha ao editar
    } else {
      setForm({
        fullName: '',
        email: '',
        phone: '',
        password: '',
        document: '',
        //clientId: '',
        userRole: 0,
      });
    }
  }, [initialData, open]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleRoleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm(prev => ({ ...prev, userRole: Number(e.target.value) }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(form);
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{isEdit ? 'Editar Usuário' : 'Adicionar Usuário'}</DialogTitle>
      <form onSubmit={handleSubmit}>
        <DialogContent className="flex flex-col gap-4">
          <TextField label="Nome completo" name="fullName" value={form.fullName} onChange={handleChange} required fullWidth />
          <TextField label="Email" name="email" value={form.email} onChange={handleChange} required fullWidth type="email" />
          <TextField label="Telefone" name="phone" value={form.phone} onChange={handleChange} fullWidth />
          <TextField label="Documento" name="document" value={form.document} onChange={handleChange} fullWidth />
          {/*<TextField label="ID do Cliente" name="clientId" value={form.clientId} onChange={handleChange} fullWidth />*/}
          {!isEdit && (
            <TextField label="Senha" name="password" value={form.password} onChange={handleChange} type="password" required fullWidth />
          )}
          <TextField select label="Perfil" name="userRole" value={form.userRole} onChange={handleRoleChange} required fullWidth>
            <MenuItem value={1}>Admin</MenuItem>
            <MenuItem value={0}>Cliente</MenuItem>
          </TextField>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancelar</Button>
          <Button type="submit" variant="contained" color="primary">{isEdit ? 'Salvar' : 'Adicionar'}</Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

export default UserFormModal; 