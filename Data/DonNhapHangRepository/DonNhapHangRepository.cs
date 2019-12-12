﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFFMS.API.Dtos.DonNhapHangDto;
using MFFMS.API.Helpers;
using MFFMS.API.Helpers.Params;
using MFFMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MFFMS.API.Data.DonNhapHangRepository
{
    public class DonNhapHangRepository : IDonNhapHangRepository
    {
        private readonly DataContext _context;
        private int _totalItems;
        private int _totalPages;

        public DonNhapHangRepository(DataContext context)
        {
            _context = context;
            _totalItems = 0;
            _totalPages = 0;
        }

        public async Task<DonNhapHang> Create(DonNhapHangForCreateDto donNhapHang)
        {
            var danhSachDonNhapHang = await _context.DanhSachDonNhapHang.OrderByDescending(x=>x.MaDonNhapHang).FirstOrDefaultAsync();
            var maDonNhapHang = 1;
            if(danhSachDonNhapHang == null)
            {
                maDonNhapHang = 1;
            }
            else
            {
                maDonNhapHang = danhSachDonNhapHang.MaDonNhapHang + 1;
            }

            var newDonNhapHang = new DonNhapHang
            {
                MaDonNhapHang = maDonNhapHang,
                MaNhaCungCap = donNhapHang.MaNhaCungCap,
                MaNhanVien = donNhapHang.MaNhanVien,
                NgayGiaoHang = donNhapHang.NgayGiaoHang,
                NoiNhanHang = donNhapHang.NoiNhanHang,
                ThoiGianCapNhat = DateTime.Now,
                ThoiGianTao = DateTime.Now,
                TrangThai = 1,
                DaXoa = 0
            };

            await _context.DanhSachDonNhapHang.AddAsync(newDonNhapHang);
            await _context.SaveChangesAsync();
            return newDonNhapHang;
        }

        public async Task<PagedList<DonNhapHang>> GetAll(DonNhapHangParams userParams)
        {
            var result = _context.DanhSachDonNhapHang.Include(x=>x.NhaCungCap).Include(x=>x.NhanVien).AsQueryable();
            var sortField = userParams.SortField;
            var sortOrder = userParams.SortOrder;
            var keyword = userParams.Keyword;
            var thoiGianTaoBatDau = userParams.ThoiGianTaoBatDau;
            var thoiGianTaoKetThuc = userParams.ThoiGianTaoKetThuc;
            var thoiGianCapNhatBatDau = userParams.ThoiGianCapNhatBatDau;
            var thoiGianCapNhatKetThuc = userParams.ThoiGianCapNhatKetThuc;
            var trangThai = userParams.TrangThai;
            var daXoa = userParams.DaXoa;
            var ngayGiaoHangBatDau = userParams.NgayGiaoHangBatDau;
            var ngayGiaoHangKetThuc = userParams.NgayGiaoHangKetThuc;

            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(x => x.NoiNhanHang.ToLower().Contains(keyword.ToLower()) || x.MaNhanVien.ToLower().Contains(keyword.ToLower())|| x.MaDonNhapHang.ToString() == keyword || x.MaDonNhapHang.ToString() == keyword);
            }

            if (thoiGianTaoBatDau.GetHashCode() != 0 && thoiGianTaoKetThuc.GetHashCode() != 0)
            {
                result = result.Where(x => x.ThoiGianTao >= thoiGianTaoBatDau && x.ThoiGianTao <= thoiGianTaoKetThuc);
            }

            if (thoiGianCapNhatBatDau.GetHashCode() != 0 && thoiGianCapNhatKetThuc.GetHashCode() != 0)
            {
                result = result.Where(x => x.ThoiGianCapNhat >= thoiGianCapNhatBatDau && x.ThoiGianCapNhat <= thoiGianCapNhatKetThuc);
            }

            if (trangThai == -1 || trangThai == 1)
            {
                result = result.Where(x => x.TrangThai == trangThai);
            }

            if (daXoa == 0 || daXoa == 1)
            {
                result = result.Where(x => x.DaXoa == daXoa);
            }

            
            if (ngayGiaoHangBatDau.GetHashCode() != 0 && ngayGiaoHangKetThuc.GetHashCode() != 0)
            {
                result = result.Where(x => x.NgayGiaoHang >= ngayGiaoHangKetThuc && x.NgayGiaoHang <= ngayGiaoHangKetThuc);
            }

            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "MaDonNhapHang":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.MaDonNhapHang);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.MaDonNhapHang);
                        }
                        break;
                    case "MaNhaCungCap":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.MaNhaCungCap);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.MaNhaCungCap);
                        }
                        break;
                    case "MaNhanVien":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.MaNhanVien);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.MaNhanVien);
                        }
                        break;

                    case "NgayGiaoHang":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.NgayGiaoHang);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.NgayGiaoHang);
                        }
                        break;

                    case "NoiNhanHang":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.NoiNhanHang);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.NoiNhanHang);
                        }
                        break;


                    case "ThoiGianTao":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.ThoiGianTao);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.ThoiGianTao);
                        }
                        break;

                    case "ThoiGianCapNhat":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.ThoiGianCapNhat);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.ThoiGianCapNhat);
                        }
                        break;

                    case "TrangThai":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.TrangThai);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.TrangThai);
                        }
                        break;

                    default:
                        result = result.OrderByDescending(x => x.ThoiGianTao);
                        break;
                }
            }

            _totalItems = result.Count();
            _totalPages = (int)Math.Ceiling((double)_totalItems / (double)userParams.PageSize);

            return await PagedList<DonNhapHang>.CreateAsync(result, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<DonNhapHang> GetById(int id)
        {
            var result = await _context.DanhSachDonNhapHang.Include(x => x.NhanVien).Include(x => x.NhaCungCap).FirstOrDefaultAsync(x => x.MaDonNhapHang == id);
            return result;
        }

        public object GetStatusStatistics(DonNhapHangParams userParams)
        {
            var result = _context.DanhSachDonNhapHang.Include(x => x.NhaCungCap).Include(x => x.NhanVien).AsQueryable();
            var sortField = userParams.SortField;
            var sortOrder = userParams.SortOrder;
            var keyword = userParams.Keyword;
            var thoiGianTaoBatDau = userParams.ThoiGianTaoBatDau;
            var thoiGianTaoKetThuc = userParams.ThoiGianTaoKetThuc;
            var thoiGianCapNhatBatDau = userParams.ThoiGianCapNhatBatDau;
            var thoiGianCapNhatKetThuc = userParams.ThoiGianCapNhatKetThuc;
            var trangThai = userParams.TrangThai;
            var daXoa = userParams.DaXoa;
            var ngayGiaoHangBatDau = userParams.NgayGiaoHangBatDau;
            var ngayGiaoHangKetThuc = userParams.NgayGiaoHangKetThuc;

            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(x => x.NoiNhanHang.ToLower().Contains(keyword.ToLower()) || x.MaNhanVien.ToLower().Contains(keyword.ToLower()) || x.MaDonNhapHang.ToString() == keyword || x.MaDonNhapHang.ToString() == keyword);
            }

            if (thoiGianTaoBatDau.GetHashCode() != 0 && thoiGianTaoKetThuc.GetHashCode() != 0)
            {
                result = result.Where(x => x.ThoiGianTao >= thoiGianTaoBatDau && x.ThoiGianTao <= thoiGianTaoKetThuc);
            }

            if (thoiGianCapNhatBatDau.GetHashCode() != 0 && thoiGianCapNhatKetThuc.GetHashCode() != 0)
            {
                result = result.Where(x => x.ThoiGianCapNhat >= thoiGianCapNhatBatDau && x.ThoiGianCapNhat <= thoiGianCapNhatKetThuc);
            }

            if (trangThai == -1 || trangThai == 1)
            {
                result = result.Where(x => x.TrangThai == trangThai);
            }

            if (daXoa == 0 || daXoa == 1)
            {
                result = result.Where(x => x.DaXoa == daXoa);
            }

            if (ngayGiaoHangBatDau.GetHashCode() != 0 && ngayGiaoHangKetThuc.GetHashCode() != 0)
            {
                result = result.Where(x => x.NgayGiaoHang >= ngayGiaoHangKetThuc && x.NgayGiaoHang <= ngayGiaoHangKetThuc);
            }

            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "MaDonNhapHang":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.MaDonNhapHang);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.MaDonNhapHang);
                        }
                        break;
                    case "MaNhaCungCap":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.MaNhaCungCap);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.MaNhaCungCap);
                        }
                        break;
                    case "MaNhanVien":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.MaNhanVien);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.MaNhanVien);
                        }
                        break;

                    case "NgayGiaoHang":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.NgayGiaoHang);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.NgayGiaoHang);
                        }
                        break;

                    case "NoiNhanHang":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.NoiNhanHang);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.NoiNhanHang);
                        }
                        break;


                    case "ThoiGianTao":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.ThoiGianTao);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.ThoiGianTao);
                        }
                        break;

                    case "ThoiGianCapNhat":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.ThoiGianCapNhat);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.ThoiGianCapNhat);
                        }
                        break;

                    case "TrangThai":
                        if (string.Equals(sortOrder, "ASC", StringComparison.OrdinalIgnoreCase))
                        {
                            result = result.OrderBy(x => x.TrangThai);
                        }
                        else
                        {
                            result = result.OrderByDescending(x => x.TrangThai);
                        }
                        break;

                    default:
                        result = result.OrderByDescending(x => x.ThoiGianTao);
                        break;
                }
            }


            var all = result.Count();
            var active = result.Count(x => x.DaXoa == 0);
            var inactive = result.Count(x => x.DaXoa == 1);

            return new
            {
                All = all,
                Active = active,
                Inactive = inactive
            };
        }

        public int GetTotalItems()
        {
            return _totalItems;
        }

        public int GetTotalPages()
        {
            return _totalPages;
        }

        public async Task<DonNhapHang> PermanentlyDeleteById(int id)
        {
            var donNhapHangToDelete = await _context.DanhSachDonNhapHang.FirstOrDefaultAsync(x => x.MaDonNhapHang == id);
            _context.DanhSachDonNhapHang.Remove(donNhapHangToDelete);
            await _context.SaveChangesAsync();
            return donNhapHangToDelete;
        }

        public async Task<DonNhapHang> RestoreById(int id)
        {
            var donNhapHangToRestoreById = await _context.DanhSachDonNhapHang.FirstOrDefaultAsync(x => x.MaDonNhapHang == id);
            donNhapHangToRestoreById.DaXoa = 0;
            donNhapHangToRestoreById.ThoiGianCapNhat = DateTime.Now;
            _context.DanhSachDonNhapHang.Update(donNhapHangToRestoreById);
            await _context.SaveChangesAsync();
            return donNhapHangToRestoreById;
        }

        public async Task<DonNhapHang> TemporarilyDeleteById(int id)
        {
            var donNhapHangToTemporarilyDeleteById = await _context.DanhSachDonNhapHang.FirstOrDefaultAsync(x => x.MaDonNhapHang == id);
            donNhapHangToTemporarilyDeleteById.DaXoa = 1;
            donNhapHangToTemporarilyDeleteById.ThoiGianCapNhat = DateTime.Now;
            _context.DanhSachDonNhapHang.Update(donNhapHangToTemporarilyDeleteById);
            await _context.SaveChangesAsync();
            return donNhapHangToTemporarilyDeleteById;
        }

        public async Task<DonNhapHang> UpdateById(int id, DonNhapHangForUpdateDto donNhapHang)
        {
            var oldRecord = await _context.DanhSachDonNhapHang.AsNoTracking().FirstOrDefaultAsync(x => x.MaDonNhapHang == id);
            var donNhapHangToUpdateById = new DonNhapHang
            {
                MaDonNhapHang = id,
                MaNhaCungCap = donNhapHang.MaNhaCungCap,
                MaNhanVien = donNhapHang.MaNhanVien,
                NgayGiaoHang = donNhapHang.NgayGiaoHang,
                NoiNhanHang = donNhapHang.NoiNhanHang,
                TrangThai = donNhapHang.TrangThai,
                ThoiGianTao = oldRecord.ThoiGianTao,
                DaXoa = oldRecord.DaXoa
            };

            _context.DanhSachDonNhapHang.Update(donNhapHangToUpdateById);
            await _context.SaveChangesAsync();
            return donNhapHangToUpdateById;
        }
    }
}