import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Typography, Paper, List, ListItem, ListItemText, Button, CircularProgress, Box, Pagination } from '@mui/material';
import { contractsService } from '../services/contractsService';
import Snackbar from '@mui/material/Snackbar';
import MuiAlert from '@mui/material/Alert';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';

const ContractDetail: React.FC = () => {
  const { uuid } = useParams<{ uuid: string }>();
  const [contract, setContract] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const itemsPerPage = 30;
  const [snackbar, setSnackbar] = useState<{open: boolean, message: string, severity: 'success'|'error'}>({open: false, message: '', severity: 'success'});
  const user = useSelector((state: RootState) => state.auth.user);

  useEffect(() => {
    const fetchContract = async () => {
      setLoading(true);
      try {
        if (!uuid) throw new Error('UUID inválido');
        const data = await contractsService.getById(uuid);
        setContract(data);
      } catch {
        setError('Erro ao buscar detalhes do contrato');
      } finally {
        setLoading(false);
      }
    };
    fetchContract();
  }, [uuid]);

  if (loading) return <Box className="flex justify-center mt-10"><CircularProgress /></Box>;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!contract) return null;

  const paginatedInstallments = contract?.installments?.slice((page - 1) * itemsPerPage, page * itemsPerPage) || [];
  const pageCount = contract?.installments ? Math.ceil(contract.installments.length / itemsPerPage) : 1;

  const handleRequestAdvance = async (installmentId: string) => {
    try {
      await contractsService.advancedRequest(installmentId);
      setSnackbar({open: true, message: 'Solicitação enviada com sucesso!', severity: 'success'});
    } catch (err: any) {
      setSnackbar({open: true, message: 'Erro ao solicitar antecipação.', severity: 'error'});
    }
  };

  return (
    <Box className="min-h-screen w-screen flex flex-col items-center bg-gray-100 py-8">
      <Paper className="w-full max-w-3xl p-8 mb-8">
        <Button variant="outlined" onClick={() => navigate(-1)} className="mb-4">Voltar</Button>
        <Typography variant="h5" gutterBottom>Detalhes do Contrato</Typography>
        <Typography><b>Número:</b> {contract.contractNumber}</Typography>
        <Typography><b>Cliente:</b> {contract.user?.fullName} ({contract.user?.email})</Typography>
        <Typography><b>Valor Inicial:</b> {contract.initialAmount?.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</Typography>
        <Typography><b>Valor Restante:</b> {contract.remainingValue?.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</Typography>
        <Typography><b>Início:</b> {contract.startDate && new Date(contract.startDate).toLocaleDateString()}</Typography>
        <Typography><b>Fim:</b> {contract.endDate && new Date(contract.endDate).toLocaleDateString()}</Typography>
        <Typography><b>Ativo:</b> {contract.isActive ? 'Sim' : 'Não'}</Typography>
        <Typography><b>Finalizado:</b> {contract.isFinished ? 'Sim' : 'Não'}</Typography>
        <Typography variant="h6" className="mt-6">Parcelas</Typography>
        {contract.installments && contract.installments.length > 0 ? (
          <>
            <List>
              {paginatedInstallments.map((inst: any, idx: number) => (
                <ListItem key={inst.uuid || idx} divider className={idx % 2 === 0 ? 'bg-gray-50' : 'bg-white'}>
                  <ListItemText
                    primary={
                      <span className="flex flex-col md:flex-row md:items-center md:gap-4">
                        <span className="font-semibold">Vencimento: {new Date(inst.dueDate).toLocaleDateString()}</span>
                        <span className="font-bold text-green-700">Valor: {Number(inst.value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
                      </span>
                    }
                    secondary={
                      <span className={
                        inst.status === 1
                          ? 'text-green-600 font-semibold'
                          : inst.status === 0
                          ? 'text-yellow-600 font-semibold'
                          : 'text-gray-500'
                      }>
                        Status: {inst.status === 0 ? 'Pendente' : inst.status === 1 ? 'Pago' : 'Outro'}
                      </span>
                    }
                  />
                  <Button
                    variant="contained"
                    color="primary"
                    size="small"
                    style={{ marginLeft: 16 }}
                    onClick={() => handleRequestAdvance(inst.uuid)}
                    disabled={
                      user?.role !== 'Client' ||
                      inst.status !== 0 ||
                      inst.isAntecipated ||
                      (inst.antecipationStatus != null && inst.antecipationStatus > 0) ||
                      (new Date(inst.dueDate).getTime() - Date.now()) / (1000 * 60 * 60 * 24) <= 30
                    }
                    hidden={user?.role !== 'Client'}
                  >
                    Solicitar Antecipação
                  </Button>
                </ListItem>
              ))}
            </List>
            {pageCount > 1 && (
              <Box className="flex justify-center mt-4">
                <Pagination count={pageCount} page={page} onChange={(_, value) => setPage(value)} color="primary" />
              </Box>
            )}
          </>
        ) : (
          <Typography>Nenhuma parcela encontrada.</Typography>
        )}
      </Paper>
      <Snackbar open={snackbar.open} autoHideDuration={3000} onClose={() => setSnackbar({...snackbar, open: false})}>
        <MuiAlert onClose={() => setSnackbar({...snackbar, open: false})} severity={snackbar.severity} sx={{ width: '100%' }}>
          {snackbar.message}
        </MuiAlert>
      </Snackbar>
    </Box>
  );
};

export default ContractDetail; 