using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace CatalogsImplement
{
    internal static class ManageCache
    {
        private static ObjectCache cache = MemoryCache.Default;

        internal static List<CatalogsContract.CatalogGeneric> GetCacheCatalog(string pKeyCatalog, string pParam = "")
        {
            List<CatalogsContract.CatalogGeneric> cat = new List<CatalogsContract.CatalogGeneric>();

            if (cache[pKeyCatalog] != null && string.IsNullOrEmpty(pParam))
            {
                cat = cache[pKeyCatalog] as List<CatalogsContract.CatalogGeneric>;
            }
            else
            {
                int durcache = 0;
                string filecache = System.Configuration.ConfigurationManager.AppSettings["filecache"].ToString();
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["durationcache"].ToString(), out durcache);

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(durcache);

                if (!string.IsNullOrEmpty(filecache))
                    policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string>() { filecache }));

                cat = GetCatalog(pKeyCatalog, pParam);
                cache.Set(pKeyCatalog, cat, policy);
            }

            return cat;
        }

        private static List<CatalogsContract.CatalogGeneric> GetCatalog(string pKeyCatalog, string pParam = "")
        {
            List<CatalogsContract.CatalogGeneric> catdb = new List<CatalogsContract.CatalogGeneric>();

            using (EntityData.AMBDV8Entities entities = new EntityData.AMBDV8Entities())
            {
                switch (pKeyCatalog.Trim().ToUpper())
                {
                    case "JOBSCATALOG":
                        entities.AMPuestos.Where((w) => w.Preconfigurated == true).OrderBy((o) => o.Nombre).ToList().ForEach((c) =>
                        {
                            catdb.Add(new CatalogsContract.CatalogGeneric()
                            {
                                Descripcion = c.Nombre.Trim().ToUpper(),
                                Id = c.idPuesto
                            });
                        });
                        break;

                    case "PAISESCATALOG":
                        entities.AMPaises.ToList().ForEach((c) =>
                        {
                            catdb.Add(new CatalogsContract.CatalogGeneric()
                            {
                                Descripcion = c.Nombre
                                 ,
                                Id = c.idPais
                            });
                        });
                        break;

                    case "LENGCATALOG":
                        entities.AMLengTest.ToList().ForEach((c) =>
                        {
                            catdb.Add(new CatalogsContract.CatalogGeneric()
                            {
                                Descripcion = c.Lenguaje
                                 ,
                                Id = c.idLeng
                            });
                        });
                        break;

                    case "ESTATECATALOG":
                        int pKeyCountry = Convert.ToInt32(pParam);
                        entities.AMEstados.Where(w => w.idPais == pKeyCountry).ToList().ForEach((c) =>
                        {
                            catdb.Add(new CatalogsContract.CatalogGeneric()
                            {
                                Descripcion = c.Nombre
                                ,
                                Id = c.idEstado
                            });
                        });
                        break;

                    case "NATIONALITYCATALOG":
                        entities.AMNacionalidad.ToList().ForEach((c) =>
                        {
                            catdb.Add(new CatalogsContract.CatalogGeneric()
                            {
                                Descripcion = c.Nacionalidad
                                ,
                                Id = c.idNacionalidad
                            });
                        });
                        break;

                }

            }

            using (EntityData.AdminsitradorDBEntities entities = new EntityData.AdminsitradorDBEntities())
            {
                switch (pKeyCatalog.Trim().ToUpper())
                {
                    case "LICENCECATALOG":
                        entities.Licenciamiento.ToList().ForEach((c) =>
                        {
                            catdb.Add(new CatalogsContract.CatalogGeneric()
                            {
                                Descripcion = c.Descripcion
                                 ,
                                Id = c.LicenciamientoID
                            });
                        });
                        break;

                    case "PRODUCTCATALOG":
                        entities.PRODUCTOS.ToList().ForEach((c) =>
                        {
                            catdb.Add(new CatalogsContract.CatalogGeneric()
                            {
                                Descripcion = c.prodDescription
                                 ,
                                Id = c.prodId
                            });
                        });
                        break;
                }
            }

            switch (pKeyCatalog.Trim().ToUpper())
            {
                case "GENRECATALOG":
                    catdb.Add(new CatalogsContract.CatalogGeneric() { Id = 1, Descripcion = "Masculino" });
                    catdb.Add(new CatalogsContract.CatalogGeneric() { Id = 2, Descripcion = "Femenino" });
                    break;

                case "STATUSCANDIDATECATALOG":
                    catdb.Add(new CatalogsContract.CatalogGeneric() { Id = 1, Descripcion = "Candidato" });
                    catdb.Add(new CatalogsContract.CatalogGeneric() { Id = 2, Descripcion = "Empleado" });
                    catdb.Add(new CatalogsContract.CatalogGeneric() { Id = 3, Descripcion = "Ex-empleado" });
                    break;
            }

            return catdb;
        }

    }
}