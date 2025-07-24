import React, { useEffect, useState } from 'react';
import { Typography, Paper, List, ListItem, ListItemText, CircularProgress, Box, Pagination, Button } from '@mui/material';
import { contractsService } from '../services/contractsService';

const MinhasSolicitacoes: React.FC = () => {
  const [solicitacoes, setSolicitacoes] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [total, setTotal] = useState(0);

  const fetchSolicitacoes = async (pagerNumber = 1) => {
    setLoading(true);
    try {
      const response = await contractsService.getAdvancedRequests(pagerNumber, pageSize);
      setSolicitacoes(response.data || []);
      setTotal(response.totalCount || response.data?.length || 0);
    } catch (err: any) {
      setError('Erro ao buscar solicitações');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSolicitacoes(page);
    // eslint-disable-next-line
  }, [page]);

  if (loading) return <div className="flex justify-center mt-10"><CircularProgress /></div>;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <Box className="min-h-screen w-screen flex flex-col items-center bg-gray-100 py-8">
      <Paper className="w-full max-w-3xl p-8 mb-8">
        <Typography variant="h5" gutterBottom>Minhas Solicitações de Antecipação</Typography>
        {solicitacoes.length === 0 ? (
          <Typography>Nenhuma solicitação encontrada.</Typography>
        ) : (
          <List>
            {solicitacoes.map((sol: any, idx: number) => (
              <Box key={sol.contractId + idx} mb={2}>
                <Typography variant="subtitle1" fontWeight={600}>
                  Contrato: {sol.contractNumber}
                </Typography>
                <Typography variant="body2" color="textSecondary" mb={1}>
                  Cliente: {sol.clientName}
                </Typography>
                {sol.installments.map((inst: any, i: number) => (
                  <ListItem key={inst.uuid || i} divider className={i % 2 === 0 ? 'bg-gray-50' : 'bg-white'}>
                    <ListItemText
                      primary={
                        <span className="flex flex-col md:flex-row md:items-center md:gap-4">
                          <span className="font-semibold">Vencimento: {new Date(inst.dueDate).toLocaleDateString()}</span>
                          <span className="font-bold text-green-700">Valor: {Number(inst.value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
                        </span>
                      }
                      secondary={
                        <span className={
                          inst.antecipationStatus === 1 ? 'text-orange-700 font-semibold' :
                          inst.antecipationStatus === 2 ? 'text-green-700 font-semibold' :
                          inst.antecipationStatus === 3 ? 'text-red-700 font-semibold' :
                          'text-gray-500'
                        }>
                          {inst.antecipationStatus === 1 && 'Solicitação Pendente'}
                          {inst.antecipationStatus === 2 && 'Solicitação Aprovada'}
                          {inst.antecipationStatus === 3 && 'Solicitação Rejeitada'}
                          {(inst.antecipationStatus == null || inst.antecipationStatus === 0) && 'Nenhuma solicitação'}
                        </span>
                      }
                    />
                  </ListItem>
                ))}
              </Box>
            ))}
          </List>
        )}
        {total > pageSize && (
          <Box className="flex justify-center mt-4">
            <Pagination count={Math.ceil(total / pageSize)} page={page} onChange={(_, value) => setPage(value)} color="primary" />
          </Box>
        )}
      </Paper>
    </Box>
  );
};

export default MinhasSolicitacoes; 