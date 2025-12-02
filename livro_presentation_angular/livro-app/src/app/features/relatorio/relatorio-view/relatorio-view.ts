import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RelatorioService } from '../../../core/services/relatorio.service';
import { RelatorioLivro } from '../../../core/models';
import { ToastService } from '../../../core/services/toast.service';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-relatorio-view',
  imports: [CommonModule],
  templateUrl: './relatorio-view.html',
  styleUrl: './relatorio-view.css'
})
export class RelatorioView implements OnInit {
  relatorio = signal<RelatorioLivro[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  constructor(
    private relatorioService: RelatorioService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadRelatorio();
  }

  loadRelatorio(): void {
    console.log('=== INICIO loadRelatorio ===');
    this.loading.set(true);
    this.error.set(null);
    console.log('Loading definido como true:', this.loading());
    
    this.relatorioService.getRelatorio().subscribe({
      next: (response: any) => {
        console.log('=== CALLBACK NEXT EXECUTADO ===');
        console.log('Response:', response);
        const data = response?.resultData || [];
        console.log('Data extraída:', data);
        console.log('Data é array?', Array.isArray(data));
        
        this.relatorio.set(Array.isArray(data) ? data : []);
        console.log('Relatório setado. Length:', this.relatorio().length);
        
        this.loading.set(false);
        console.log('Loading definido como false:', this.loading());
        console.log('=== FIM CALLBACK NEXT ===');
      },
      error: (err) => {
        console.log('=== ERRO ===', err);
        this.error.set('Erro ao carregar relatório');
        this.loading.set(false);
        this.toastService.error('Erro ao carregar relatório');
      }
    });
    console.log('=== Subscription criada ===');
  }

  print(): void {
    window.print();
  }

  exportarPDF(): void {
    const doc = new jsPDF();
    
    // Título
    doc.setFontSize(18);
    doc.text('Relatório de Livros por Autor', 14, 22);
    
    // Data
    doc.setFontSize(10);
    doc.text(`Gerado em: ${new Date().toLocaleString('pt-BR')}`, 14, 30);

    // Dados da tabela
    const tableData = this.relatorio().map(item => [
      item.autorNome,
      item.livroTitulo,
      item.editora,
      item.edicao?.toString() || '',
      item.anoPublicacao,
      item.assuntos.join(', '),
      this.formatarValorTotal(item.valores)
    ]);

    autoTable(doc, {
      head: [['Autor', 'Título', 'Editora', 'Edição', 'Ano', 'Assuntos', 'Valor Total']],
      body: tableData,
      startY: 35,
      styles: { fontSize: 8 },
      headStyles: { fillColor: [52, 58, 64] },
      columnStyles: {
        0: { cellWidth: 30 },
        1: { cellWidth: 35 },
        2: { cellWidth: 30 },
        3: { cellWidth: 15 },
        4: { cellWidth: 15 },
        5: { cellWidth: 35 },
        6: { cellWidth: 25, halign: 'right' }
      }
    });

    // Salvar PDF
    doc.save(`relatorio-livros-${new Date().getTime()}.pdf`);
  }

  formatarValorTotal(valores: any[]): string {
    if (!valores || valores.length === 0) return 'R$ 0,00';
    const total = valores.reduce((sum, v) => sum + (v.valor || 0), 0);
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(total);
  }

  getAssuntosString(assuntos: string[]): string {
    return assuntos.join(', ');
  }
}
