"use client";

import React, { useState, useEffect } from "react";
import axios from "axios";
import { FaTrash, FaEdit, FaPlus, FaSave, FaTimes } from "react-icons/fa";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent } from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import { AxiosError } from 'axios';

interface Todo {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
}

interface TodoBase {
  title: string;
  description: string;
}

function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [newTodo, setNewTodo] = useState<TodoBase>({ title: "", description: "" });
  const [editId, setEditId] = useState<number | null>(null);
  const [editTodo, setEditTodo] = useState<TodoBase>({ title: "", description: "" });

  useEffect(() => {
    fetchTodos();
  }, []);

  // Fetch all todos from the API
  const fetchTodos = async () => {
    try {
      const { data } = await axios.get<Todo[]>("http://localhost:5223/api/todo");
      setTodos(data);
    } catch (error) {
      toast.error("Failed to fetch todos");
    }
  };

  // Add a new todo
  const handleAddTodo = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newTodo.title.trim()) {
      toast.error("Title is required");
      return;
    }
    try {
      await axios.post("http://localhost:5223/api/todo", newTodo);
      setNewTodo({ title: "", description: "" });
      fetchTodos();
      toast.success("Todo added successfully!");
    } catch (error) {
      toast.error("Failed to add todo");
    }
  };

  // Delete a todo
  const handleDeleteTodo = async (id: number) => {
    try {
      await axios.delete(`http://localhost:5223/api/todo/${id}`);
      fetchTodos();
      toast.success("Todo deleted successfully!");
    } catch (error) {
      toast.error("Failed to delete todo");
    }
  };

  // Toggle completion status using the toggle route
  const handleToggleComplete = async (id: number) => {
    try {
      await axios.put(`http://localhost:5223/api/todo/toggle/${id}`); // Using the toggle route
      fetchTodos();
    } catch (error) {
      toast.error("Failed to update todo");
    }
  };

  // Handle editing a todo
  const handleEditClick = (todo: Todo) => {
    setEditId(todo.id);
    setEditTodo({ title: todo.title, description: todo.description });
  };

  // Save edited todo
  const handleSaveEdit = async () => {
    if (!editTodo.title.trim()) {
      toast.error("Title is required");
      return;
    }
    const updatedTodo = {
      id: editId,           // Include the id in the request body
      title: editTodo.title,
      description: editTodo.description,
    };
  
    try {
      await axios.put(`http://localhost:5223/api/todo/${editId}`, updatedTodo);
      setEditId(null);
      fetchTodos();
      toast.success("Todo updated successfully!");
    } catch (error) {
      // Type narrowing to check if error is an instance of AxiosError
      if (axios.isAxiosError(error)) {
        toast.error(`Failed to update todo: ${error.response?.data?.message || "Unknown error"}`);
      } else {
        toast.error("An unexpected error occurred.");
      }
    }
  };

  return (
    <div className="max-w-lg mx-auto py-8">
      <ToastContainer position="top-center" autoClose={3000} />
      <h1 className="text-center text-2xl font-bold mb-4">Todo Manager</h1>

      {/* Add New Todo */}
      <Card className="mb-4 pt-4">
        <CardContent>
          <form onSubmit={handleAddTodo} className="flex gap-2">
            <Input
              placeholder="Enter title"
              value={newTodo.title}
              onChange={(e) => setNewTodo({ ...newTodo, title: e.target.value })}
            />
            <Input
              placeholder="Enter description"
              value={newTodo.description}
              onChange={(e) => setNewTodo({ ...newTodo, description: e.target.value })}
            />
            <Button type="submit">
              <FaPlus className="mr-2" /> Add
            </Button>
          </form>
        </CardContent>
      </Card>

      {/* Display Todos */}
      {todos.length === 0 ? (
        <p className="text-center text-gray-500">No todos found. Start by adding one!</p>
      ) : (
        <div className="space-y-2">
          {todos.map((todo) => (
            <Card key={todo.id} className="flex justify-between items-center p-4">
              <div className="flex items-center gap-4">
                <Checkbox
                  checked={todo.isCompleted}
                  onCheckedChange={() => handleToggleComplete(todo.id)} // Toggling completion status
                />
                {editId === todo.id ? (
                  <div>
                    <Input
                      value={editTodo.title}
                      onChange={(e) => setEditTodo({ ...editTodo, title: e.target.value })}
                    />
                    <Input
                      value={editTodo.description}
                      onChange={(e) => setEditTodo({ ...editTodo, description: e.target.value })}
                    />
                  </div>
                ) : (
                  <div>
                    <h5 className={todo.isCompleted ? "line-through text-gray-500" : "font-medium"}>
                      {todo.title}
                    </h5>
                    {todo.description && (
                      <p className={todo.isCompleted ? "line-through text-gray-400" : "text-gray-700"}>
                        {todo.description}
                      </p>
                    )}
                  </div>
                )}
              </div>

              {/* Action Buttons */}
              <div className="flex gap-2">
                {editId === todo.id ? (
                  <>
                    <Button onClick={handleSaveEdit} variant="default">
                      <FaSave />
                    </Button>
                    <Button onClick={() => setEditId(null)} variant="outline">
                      <FaTimes />
                    </Button>
                  </>
                ) : (
                  <>
                    <Button onClick={() => handleEditClick(todo)} variant="outline">
                      <FaEdit />
                    </Button>
                    <Button onClick={() => handleDeleteTodo(todo.id)} variant="destructive">
                      <FaTrash />
                    </Button>
                  </>
                )}
              </div>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}

export default App;
