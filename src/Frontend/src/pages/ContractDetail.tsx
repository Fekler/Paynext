import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Typography, Paper, List, ListItem, ListItemText, Button, CircularProgress, Box } from '@mui/material';
import api from '../api';

const ContractDetail: React.FC = () => {
  const { uuid } = useParams<{ uuid: string }>();
  const [contract, setContract] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchContract = async () => {
      setLoading(true);
      try {
        const response = await api.get(`/api/v1/Contracts/${uuid}`);
        setContract(response.data.data || response.data);
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
          <List>
            {contract.installments.map((inst: any, idx: number) => (
              <ListItem key={inst.uuid || idx} divider>
                <ListItemText
                  primary={`Vencimento: ${new Date(inst.dueDate).toLocaleDateString()} - Valor: R$ ${inst.value}`}
                  secondary={`Status: ${inst.status === 0 ? 'Pendente' : inst.status === 1 ? 'Pago' : 'Outro'}`}
                />
              </ListItem>
            ))}
          </List>
        ) : (
          <Typography>Nenhuma parcela encontrada.</Typography>
        )}
      </Paper>
    </Box>
  );
};

export default ContractDetail; 