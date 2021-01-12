﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreModel.Data;
using StoreModel.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Proiect.Controllers
{
    public class CustomersController : Controller
    {
        private readonly StoreContext _context;
        private readonly string _baseUrl = "http://localhost:55112/api/Customers";
        public CustomersController(StoreContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var customers = JsonConvert.DeserializeObject<List<Customer>>(await response.Content.
                ReadAsStringAsync());
                return View(customers);
            }
            return NotFound();
        }
            // GET: Customers/Details/5
            public async Task<ActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return new BadRequestResult();
                }
                var client = new HttpClient();
                var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(
                    await response.Content.ReadAsStringAsync());
                    return View(customer);
                }
                return NotFound();
            }

            // GET: Customers/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Customers/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Create([Bind("CustomerID,Name,Adress")] Customer customer)
            {
                if (ModelState.IsValid) return View(customer);
                try
                {
                    var client = new HttpClient();
                    string json = JsonConvert.SerializeObject(customer);
                    var response = await client.PostAsync(_baseUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
                }
                return View(customer);
            }
            // GET: Customers/Edit/5
            public async Task<ActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return new BadRequestResult();
                }
                var client = new HttpClient();
                var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(
                    await response.Content.ReadAsStringAsync());

                    return View(customer);
                }
                return new NotFoundResult();
            }
                // POST: Customers/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to, for 
                // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<ActionResult> Edit(int id, [Bind("CustomerID,Name,Adress")] Customer customer)
            {
                if (!ModelState.IsValid) return View(customer);
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(customer);
                var response = await client.PutAsync($"{_baseUrl}/{customer.CustomerID}",
                new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View(customer);

            }

                // GET: Customers/Delete/5
                public async Task<ActionResult> Delete(int? id)
                {
                if (id == null)
                {
                    return new BadRequestResult();
                }
                var client = new HttpClient();
                var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
                    return View(customer);
                }
                return new NotFoundResult();
        }

                // POST: Customers/Delete/5
                [HttpPost]
                [ValidateAntiForgeryToken]
            public async Task<ActionResult> Delete([Bind("CustomerID")] Customer customer)
                {
                try {
                    var client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{customer.CustomerID}")
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
                    };
                    var response = await client.SendAsync(request);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message}");
                }
return View(customer);
            }

                private bool CustomerExists(int id)
                {
                    return _context.Customers.Any(e => e.CustomerID == id);
                }
            }
        }

