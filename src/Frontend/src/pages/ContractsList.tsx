import React, { useEffect, useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, CircularProgress, Button, IconButton, Tooltip, Snackbar, Alert as MuiAlert } from '@mui/material';
import { Add, Visibility } from '@mui/icons-material';
import api from '../api';
import ContractFormModal from './ContractFormModal';

interface Contract {
  uuid: string;
  contractNumber: string;
  userUuid: string;
  initialAmount: number;
  remainingValue: number;
  startDate: string;
  endDate: string;
  isFinished: boolean;
  isActive: boolean;
  user: {
    fullName: string;
    email: string;
  };
  installments: any[];
}

const ContractsList: React.FC = () => {
  const [contracts, setContracts] = useState<Contract[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [snackbar, setSnackbar] = useState<{open: boolean, message: string, severity: 'success'|'error'}>({open: false, message: '', severity: 'success'});
  const [modalOpen, setModalOpen] = useState(false);

  const fetchContracts = async () => {
    setLoading(true);
    try {
      const response = await api.get('/api/v1/Contracts');
      setContracts(response.data.data || response.data);
    } catch (err: any) {
      setError('Erro ao buscar contratos');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchContracts();
  }, []);

  const handleAdd = () => {
    setModalOpen(true);
  };

  const handleDetails = (uuid: string) => {
    setSnackbar({open: true, message: `Detalhes do contrato ${uuid}`, severity: 'info'});
  };

  const handleModalSubmit = () => {
    fetchContracts();
    setSnackbar({open: true, message: 'Contrato salvo com sucesso!', severity: 'success'});
  };

  if (loading) return <div className="flex justify-center mt-10"><CircularProgress /></div>;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!contracts || contracts.length === 0) {
    return (
      <div className="min-h-screen w-screen flex flex-col items-center justify-center bg-gray-100">
        <div className="flex flex-col items-center gap-4">
          <Typography variant="h6" className="mt-20 text-center">Nenhum contrato ativo encontrado.</Typography>
          <Button variant="contained" color="primary" startIcon={<Add />} onClick={handleAdd}>
            Adicionar Contrato
          </Button>
        </div>
        <ContractFormModal open={modalOpen} onClose={() => setModalOpen(false)} onSubmit={handleModalSubmit} />
        <Snackbar open={snackbar.open} autoHideDuration={3000} onClose={() => setSnackbar({...snackbar, open: false})}>
          <MuiAlert onClose={() => setSnackbar({...snackbar, open: false})} severity={snackbar.severity} sx={{ width: '100%' }}>
            {snackbar.message}
          </MuiAlert>
        </Snackbar>
      </div>
    );
  }

  return (
    <div className="min-h-screen w-screen flex flex-col items-center bg-gray-100">
      <div className="flex flex-col md:flex-row justify-between items-center w-full max-w-6xl mt-8 mb-4 gap-4 px-2 md:px-8">
        <Typography variant="h6">Contratos</Typography>
        <Button variant="contained" color="primary" startIcon={<Add />} onClick={handleAdd}>
          Adicionar Contrato
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
              <TableCell>Número</TableCell>
              <TableCell>Cliente</TableCell>
              <TableCell>Valor Inicial</TableCell>
              <TableCell>Parcelas</TableCell>
              <TableCell>Início</TableCell>
              <TableCell>Fim</TableCell>
              <TableCell>Ativo</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {contracts.map((contract) => (
              <TableRow key={contract.uuid}>
                <TableCell>{contract.contractNumber}</TableCell>
                <TableCell>{contract.user?.fullName}</TableCell>
                <TableCell>{contract.initialAmount.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</TableCell>
                <TableCell>{contract.installments?.length ?? 0}</TableCell>
                <TableCell>{contract.startDate && new Date(contract.startDate).toLocaleDateString()}</TableCell>
                <TableCell>{contract.endDate && new Date(contract.endDate).toLocaleDateString()}</TableCell>
                <TableCell>{contract.isActive ? 'Sim' : 'Não'}</TableCell>
                <TableCell>
                  <Tooltip title="Ver Detalhes"><IconButton onClick={() => handleDetails(contract.uuid)}><Visibility /></IconButton></Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <ContractFormModal open={modalOpen} onClose={() => setModalOpen(false)} onSubmit={handleModalSubmit} />
      <Snackbar open={snackbar.open} autoHideDuration={3000} onClose={() => setSnackbar({...snackbar, open: false})}>
        <MuiAlert onClose={() => setSnackbar({...snackbar, open: false})} severity={snackbar.severity} sx={{ width: '100%' }}>
          {snackbar.message}
        </MuiAlert>
      </Snackbar>
    </div>
  );
};

export default ContractsList; 