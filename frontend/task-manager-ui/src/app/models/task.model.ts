export interface TaskItem {
  id?: string;
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate: string;
  priority: 'Low' | 'Medium' | 'High';
  createdAt?: string;
}

export interface CreateTaskDto {
  title: string;
  description: string;
  dueDate: string;
  priority: 'Low' | 'Medium' | 'High';
}

export interface UpdateTaskDto {
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate: string;
  priority: 'Low' | 'Medium' | 'High';
}
