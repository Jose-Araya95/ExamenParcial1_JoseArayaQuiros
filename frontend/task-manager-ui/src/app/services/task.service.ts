import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskItem, CreateTaskDto, UpdateTaskDto } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl = 'http://localhost:5200/api/tareas';

  constructor(private http: HttpClient) { }

  getTasks(isCompleted?: boolean, priority?: string, sortBy?: string, search?: string): Observable<TaskItem[]> {
    let params = new HttpParams();

    if (isCompleted !== undefined && isCompleted !== null) {
      params = params.set('isCompleted', isCompleted.toString());
    }
    if (priority) {
      params = params.set('priority', priority);
    }
    if (sortBy) {
      params = params.set('sortBy', sortBy);
    }
    if (search) {
      params = params.set('search', search);
    }

    return this.http.get<TaskItem[]>(this.apiUrl, { params });
  }

  getTaskById(id: string): Observable<TaskItem> {
    return this.http.get<TaskItem>(`${this.apiUrl}/${id}`);
  }

  createTask(dto: CreateTaskDto): Observable<TaskItem> {
    return this.http.post<TaskItem>(this.apiUrl, dto);
  }

  updateTask(id: string, dto: UpdateTaskDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  deleteTask(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
