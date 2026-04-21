using AutoMapper;
using POS.Application.DTOs;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class TableService : ITableService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public TableService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TableDto>> GetAllTablesAsync(int branchId)
        {
            var tables = await _uow.Tables.FindAsync(t => t.BranchId == branchId);
            return _mapper.Map<IEnumerable<TableDto>>(tables.OrderBy(t => t.TableNumber));
        }

        public async Task<TableDto> GetByIdAsync(int id)
        {
            var table = await _uow.Tables.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Table {id} not found");
            return _mapper.Map<TableDto>(table);
        }

        public async Task<TableDto> CreateTableAsync(CreateTableDto dto, int branchId)
        {
            var table = new Table
            {
                TableNumber = dto.TableNumber,
                Capacity = dto.Capacity,
                BranchId = branchId,
                Status = TableStatus.Available,
            };
            await _uow.Tables.AddAsync(table);
            await _uow.CommitAsync();
            return _mapper.Map<TableDto>(table);
        }

        public async Task<TableDto> UpdateTableAsync(int id, UpdateTableDto dto)
        {
            var table = await _uow.Tables.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Table {id} not found");

            table.TableNumber = dto.TableNumber;
            table.Capacity = dto.Capacity;
            table.UpdatedAt = DateTime.UtcNow;

            await _uow.Tables.UpdateAsync(table);
            await _uow.CommitAsync();
            return _mapper.Map<TableDto>(table);
        }

        public async Task UpdateTableStatusAsync(int id, TableStatus status)
        {
            var table = await _uow.Tables.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Table {id} not found");

            table.Status = status;
            table.UpdatedAt = DateTime.UtcNow;

            await _uow.Tables.UpdateAsync(table);
            await _uow.CommitAsync();
        }

        public async Task DeleteTableAsync(int id)
        {
            await _uow.Tables.DeleteAsync(id);
            await _uow.CommitAsync();
        }
    }
}
