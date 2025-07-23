import React, { useState, useEffect } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, MenuItem, FormControlLabel, Switch } from '@mui/material';
import { contractsService } from '../services/contractsService';
import api from '../api';
import dayjs from 'dayjs';

interface ContractFormModalProps {
  open: boolean;
  onClose: () => void;
  onSubmit: () => void;
  initialData?: any;
  isEdit?: boolean;
}

interface UserOption {
  uuid: string;
  fullName: string;
  email: string;
}

const ContractFormModal: React.FC<ContractFormModalProps> = ({ open, onClose, onSubmit, initialData, isEdit }) => {
  const [form, setForm] = useState({
    contractNumber: '',
    description: '',
    amount: 0,
    startDate: '',
    endDate: '',
    installmentCount: 1,
    userUuid: '',
    isActive: true,
    clientId: '',
  });
  const [users, setUsers] = useState<UserOption[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (initialData) {
      setForm({ ...initialData });
    } else {
      setForm({
        contractNumber: '',
        description: '',
        amount: 0,
        startDate: '',
        endDate: '',
        installmentCount: 1,
        userUuid: '',
        isActive: true,
        clientId: '',
      });
    }
  }, [initialData, open]);

  useEffect(() => {
    // Buscar usuários ativos para seleção
    const fetchUsers = async () => {
      try {
        const response = await api.get('/api/v1/Users');
        setUsers((response.data.data || response.data).filter((u: any) => u.isActive && u.userRole === 0));
      } catch {}
    };
    fetchUsers();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    let newForm = { ...form, [name]: type === 'checkbox' ? checked : value };
    // Calcular endDate automaticamente
    if ((name === 'startDate' || name === 'installmentCount') && newForm.startDate && Number(newForm.installmentCount) > 0) {
      const start = dayjs(newForm.startDate);
      const months = Number(newForm.installmentCount) - 1;
      const end = start.add(months, 'month');
      newForm.endDate = end.format('YYYY-MM-DD');
    }
    setForm(newForm);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      if (isEdit && initialData?.uuid) {
        await contractsService.update({ ...form, uuid: initialData.uuid });
      } else {
        await contractsService.create(form);
      }
      onSubmit();
      onClose();
    } catch {
      // Tratar erro se necessário
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>{isEdit ? 'Editar Contrato' : 'Adicionar Contrato'}</DialogTitle>
      <form onSubmit={handleSubmit}>
        <DialogContent className="flex flex-col gap-4">
          <TextField label="Número do Contrato" name="contractNumber" value={form.contractNumber} onChange={handleChange} required fullWidth />
          <TextField label="Descrição" name="description" value={form.description} onChange={handleChange} fullWidth />
          <TextField label="Valor" name="amount" value={form.amount} onChange={handleChange} type="number" required fullWidth />
          <TextField label="Data de Início" name="startDate" value={form.startDate} onChange={handleChange} type="date" required fullWidth InputLabelProps={{ shrink: true }} />
          <TextField label="Data de Fim" name="endDate" value={form.endDate} onChange={handleChange} type="date" fullWidth InputLabelProps={{ shrink: true }} InputProps={{ readOnly: true }} disabled />
          <TextField label="Parcelas" name="installmentCount" value={form.installmentCount} onChange={handleChange} type="number" required fullWidth />
          <TextField select label="Usuário" name="userUuid" value={form.userUuid} onChange={handleChange} required fullWidth>
            {users.map(user => (
              <MenuItem key={user.uuid} value={user.uuid}>{user.fullName} ({user.email})</MenuItem>
            ))}
          </TextField>
          <TextField label="ID do Cliente" name="clientId" value={form.clientId} onChange={handleChange} fullWidth />
          <FormControlLabel
            control={<Switch checked={form.isActive} onChange={handleChange} name="isActive" color="primary" />}
            label="Ativo"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancelar</Button>
          <Button type="submit" variant="contained" color="primary" disabled={loading}>{isEdit ? 'Salvar' : 'Adicionar'}</Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

export default ContractFormModal; 