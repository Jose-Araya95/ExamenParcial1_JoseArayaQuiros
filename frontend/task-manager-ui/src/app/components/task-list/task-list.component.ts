import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // <-- Added ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { TaskItem, CreateTaskDto, UpdateTaskDto } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks: TaskItem[] = [];
  editingTaskId: string | null = null;
  taskForm: FormGroup;

  searchQuery: string = '';
  selectedStatus: string = 'all';
  selectedPriority: string = '';
  selectedSort: string = 'createdAt';

  constructor(
    private taskService: TaskService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef // <-- Injected ChangeDetectorRef
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
      dueDate: ['', Validators.required],
      priority: ['Medium', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    let statusFilter: boolean | undefined = undefined;
    if (this.selectedStatus === 'completed') statusFilter = true;
    if (this.selectedStatus === 'pending') statusFilter = false;

    this.taskService.getTasks(statusFilter, this.selectedPriority, this.selectedSort, this.searchQuery)
      .subscribe({
        next: (data) => {
          this.tasks = data;
          this.cdr.detectChanges(); // <-- Trigger UI render immediately
        },
        error: (err) => console.error('Failed to load tasks', err)
      });
  }

  saveTask(): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    const formVal = this.taskForm.value;

    if (this.editingTaskId) {
      const existing = this.tasks.find(t => t.id === this.editingTaskId);
      const updateDto: UpdateTaskDto = {
        title: formVal.title,
        description: formVal.description,
        dueDate: formVal.dueDate,
        priority: formVal.priority,
        isCompleted: existing ? existing.isCompleted : false
      };

      this.taskService.updateTask(this.editingTaskId, updateDto).subscribe({
        next: () => {
          this.resetForm();
          this.loadTasks();
        },
        error: (err) => console.error('Failed to update task', err)
      });
    } else {
      const createDto: CreateTaskDto = {
        title: formVal.title,
        description: formVal.description,
        dueDate: formVal.dueDate,
        priority: formVal.priority
      };

      this.taskService.createTask(createDto).subscribe({
        next: () => {
          this.resetForm();
          this.loadTasks();
        },
        error: (err) => console.error('Failed to create task', err)
      });
    }
  }

  startEdit(task: TaskItem): void {
    if (!task.id) return;
    this.editingTaskId = task.id;
    const formattedDate = new Date(task.dueDate).toISOString().substring(0, 10);

    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      dueDate: formattedDate,
      priority: task.priority
    });
    this.cdr.detectChanges();
  }

  toggleComplete(task: TaskItem): void {
    if (!task.id) return;
    const updateDto: UpdateTaskDto = {
      title: task.title,
      description: task.description,
      isCompleted: !task.isCompleted,
      dueDate: task.dueDate,
      priority: task.priority
    };

    this.taskService.updateTask(task.id, updateDto).subscribe({
      next: () => this.loadTasks(),
      error: (err) => console.error('Failed to toggle status', err)
    });
  }

  deleteTask(id?: string): void {
    if (!id) return;
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => this.loadTasks(),
        error: (err) => console.error('Failed to delete task', err)
      });
    }
  }

  resetForm(): void {
    this.editingTaskId = null;
    this.taskForm.reset({ priority: 'Medium' });
    this.cdr.detectChanges();
  }
}
