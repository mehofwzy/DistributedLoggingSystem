import { Component, OnInit } from '@angular/core';
import { LogService, LogEntry, LogResponse } from '../services/log.service';
import { FormsModule } from '@angular/forms';  // Import FormsModule
import { CommonModule } from '@angular/common';  // Import CommonModule

@Component({
  selector: 'app-log-list',
  standalone: true,
  templateUrl: './log-list.component.html',
  styleUrls: ['./log-list.component.css'],
  imports: [FormsModule, CommonModule],
})
export class LogListComponent implements OnInit {
  logs: LogEntry[] = []; // Array to hold logs from the API
  totalCount: number = 0;

  serviceFilter: string = '';
  levelFilter: string = '';
  startTimeFilter: string = '';
  endTimeFilter: string = '';

  currentPage: number = 1;
  pageSize: number = 5;
  selectedLog: LogEntry | null = null;

  constructor(private logService: LogService) { }

  ngOnInit(): void {
    this.fetchLogs(); // Fetch logs when the component loads
  }

  // Method to fetch logs
  fetchLogs(): void {
    const filters = {
      service: this.serviceFilter,
      level: this.levelFilter,
      startTime: this.startTimeFilter ? new Date(this.startTimeFilter).toISOString() : null,
      endTime: this.endTimeFilter ? new Date(this.endTimeFilter).toISOString() : null,
    };

    this.logService.getLogs(filters, this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.logs = response.logs;
        this.totalCount = response.totalCount;
        console.log('Fetched logs: ', this.logs); // Debug message to track logs

        // Check if logs for the current page exist; if not, revert to previous page
        if (this.logs.length === 0 && this.currentPage > 1) {
          console.log('No logs found for current page, reverting to previous page');
          this.currentPage--; // Go to the previous page
          this.fetchLogs(); // Refetch logs for the previous page
        }
      },
      error: (err) => console.error('Error fetching logs', err),
    });
  }

  // Total number of pages based on total count
  totalPages(): number {
    console.log('total '+Math.ceil(this.totalCount / this.pageSize)); // Debug message to track logs
    console.log('totalCount '+this.totalCount); // Debug message to track logs
    console.log('pageSize '+this.pageSize); // Debug message to track logs

    return Math.ceil(this.totalCount / this.pageSize);
  }

  // Reset to the first page when filters change
  onFilterChange(): void {
    this.currentPage = 1;
    this.fetchLogs();
  }

  // Change the page when the user clicks a page button
  onPageChange(page: number): void {
    console.log('Attempting to go to page:', page); // Debug message
    if (page > 0 && page <= this.totalPages()) {
      this.currentPage = page;
      this.fetchLogs();
    }
  }

  // Check if the next page has data
  hasNextPage(): boolean {
    return this.currentPage < this.totalPages();
  }

  // Check if the previous page exists
  hasPreviousPage(): boolean {
    return this.currentPage > 1;
  }

  onRowClick(log: LogEntry): void {
    this.selectedLog = log;
  }

  closeModal(): void {
    this.selectedLog = null;
  }
}
