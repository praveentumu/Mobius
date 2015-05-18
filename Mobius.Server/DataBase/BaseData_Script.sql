USE [MobiusServer]

INSERT INTO [MobiusServer].[dbo].[CategoryInfo]
           (CategoryName,Value)
SELECT 'Allergy','1'
Union All
SELECT 'VitalSign','2'
Union All
SELECT 'Immunization','4'
Union All
SELECT 'Medical Annotation','8'
Union All
SELECT 'Medication','16'
Union All
SELECT 'Emotional State','32'
Union All
SELECT 'Sleep Session','64'
Union All
SELECT 'Aerobic Profile','128'
Union All
SELECT 'Diabetic Profile','256'
Union All
SELECT 'Daily Dietary Intake','512'
Union All
SELECT 'Healthcare Proxy','1024'
Union All
SELECT 'Respiratory Profile','2048'
Union All
SELECT 'Others','4096'


----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[Constraints]
           ([ConsrtID]
           ,[ConsrtName]
           ,[Description])
   
SELECT 1,'Expiry','Expiry Date'
Union All
SELECT 2,'Category','Category'
Union All
SELECT 3,'Count','Count'



----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[DefaultPassword]
           ([Password]
           ,[UserTypeId]) 

SELECT 'defaultpassword',1

----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[UserType]
           ([TypeName]
           ,[TypeDesc]
           ,[TableName])

SELECT 'Administrator','Admininstrator','AdminUser'
Union All

SELECT 'Technician','Technician','AHLTAMedicUser'
Union All

SELECT 'AHLTA Patient','Patient','AHLTAPatientUser'
Union All

SELECT 'OMF User','OMF User','OMFUser'
Union All

SELECT 'OMFAdministrator','OMF Administrator',null
Union All

SELECT 'Front Desk','Front Desk','AHLTAMedicUser'
Union All

SELECT 'Provider','Provider','AHLTAMedicUser'

----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[FacilityType]
           ([TypeName]
           ,[TypeDesc])
SELECT 'Military','Military Facility'
Union All
SELECT  'OMF','OMF Facility'

----------------------------------------------------------------------------------


INSERT INTO [MobiusServer].[dbo].[FacilityInfo]
           ([Name]
           ,[City]
           ,[Address1]
           ,[Address2]
           ,[State]
           ,[FacilityTypeID]
           ,[ZIP]
           ,[Description]
           ,[DefaultPassword]
           ,[Email]
           ,[IsActive]
           ,[FacilityPublicKey]
           ,[FacilityPrivateKey]
			,[FacilityLogo])

   
SELECT 'MAMC','Tacoma','Bldg 9040','Fitzsimmons Drive','Washington',(select facilitytypeid from facilitytype where Typename='Military'),'WA98431','Madigan Army Medical Centre','defaultpassword','mamc@Rsi.com',1
,'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQCstEUBPdDvm0f8GYkz1FX91MjYZ14x4zO13+Z9LVAxQpl54vjoNvJQVZU3OzB9eANj5JwBXDDIFy2iVAHrldN0m4EpVIPi0/xU6V8iiPvb7HqN/4h/uoZUefqOnaAE1tlx3ah/m5etHBpxiQiIM+Umdjqz9FXUFNH4/afpRzsRmQIBEQ=='
,'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW2XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBnC5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2I1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bNB0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,'Whats-New-Banner.jpg'
Union All

SELECT 'Green County','Chicago','CT road','Loca drive','Chicago',(select facilitytypeid from facilitytype where Typename='OMF'),'23415','Omf facility','defaultpassword','OmfFacility@rsi.com',0
,'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQC78mrTexX/iPPp8x3J2GvNhkwaGtw+qZs4cmu7cup8y7sW/K29fGkdOsd2WvP+vJ4yGp6d7YaqGvm3JyXJzuhOXGAVXepIo+oKsXUGoRb8R76+sVYSGRdgTxfpFs+UduOId1UlakaTYblL41Dw0A8214oyH97UxlKAEa4q7qVIqwIBEQ=='
,'MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBALvyatN7Ff+I8+nzHcnYa82GTBoa3D6pmzhya7ty6nzLuxb8rb18aR06x3Za8/68njIanp3thqoa+bcnJcnO6E5cYBVd6kij6gqxdQahFvxHvr6xVhIZF2BPF+kWz5R244h3VSVqRpNhuUvjUPDQDzbXijIf3tTGUoARrirupUirAgERAoGAAUN1UQSzIfyeQGO7XCPgBotxqvxO7Q9WqeYX9TaN1qycJgRHurZnG01pCBwlyA8JPQCb9QO/gcn4NpUhYinVrQvbesoWCfgTkKW2ziv6nnMsIZcCmieZCBH5vScGE3VgW/opcRxXZK1ChkzNMpmvfmh7Zh7mX5fV+M30uYRLBZECQQDH4l38W+6nUsA158XRhWkKkaZOQwt9IWpj0ZMoyPbUdncIkbHYmowoYYPkOjXi5KC0hqziX2OR8Cep2ipTqWwlAkEA8LYedUAPFzj+MZT0wbIB0UiT5E/0GcqFeWmtgT7PLq+f0y76WeRHjPA+3tuUIHHi7U/uySNlus4Gu+E0/GRgjwJBALBeUu26h0hYEwJjF/UbXK74/Cbv3PXwTtCLvhTtjn87WfiAjd09EkG/dF/3IHzn2RfCIBMI32KmuZXendFZQU0CQBxRqTr4ej73pW8+s2IU8SeuL4RFpD9FAKTfQZa8GF/YbSfnaL9ICGrvFnSSTal2z2c2lJAiSDQYPQcLb6U4/E0CQQCSs21UOHCnZwlZNYdsHF4QZX/AoS7BtiSAlIjffRgRUAgjGpPKJ+Tce12f2qGiKgu408LmYJdh2+4rytYfl7Fo'
,'green county.jpg'


----------------------------------------------------------------------------------


INSERT INTO [MobiusServer].[dbo].[Group]
           ([Publickey]
           ,[PrivateKey]
           ,[FacilityId])
 
SELECT
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDks22N6aC0XOiSBs6ModbDv/yMAUNKHcEVOGimbjgdfi0/3t6kU6Ekd13V9mmBErf3r36wTi6vE+F8ryu12WhVoFSaZQMRu0Nvkfoap96MTmMqa9fQv+tkv6mIeZzvsP3lxHb67GXnHIsTIvEYoHvr+j7JqJnMqhYlvNWhD+NPvQIBEQ=='
,'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAOSzbY3poLRc6JIGzoyh1sO//IwBQ0odwRU4aKZuOB1+LT/e3qRToSR3XdX2aYESt/evfrBOLq8T4XyvK7XZaFWgVJplAxG7Q2+R+hqn3oxOYypr19C/62S/qYh5nO+w/eXEdvrsZeccixMi8Rige+v6PsmomcyqFiW81aEP40+9AgERAoGANc/dis2POX9FyAGaAvjnPR4dTh5qL46HyMH6gYNYf2j7lo7K+X0W22dhX4VGAECjwc7wofRHOEDpwvwKSOfcT/AVZHy1noWjuP4WLkvJheJGYlCX/Zm9xkHVpa5HMp/EzEyKdKXi6LVNXgaQGYA5GhjUx3AI4nEs4b+BcYUefuECQQD1PmnSwyprCO+tBMQxicrhntm3MBWURH+GK+VuHHfi5H58CcJzcFOrDfEAKX/GpMHrkwN9ORTmXYWI6cZbBRVPAkEA7rtFgDwFmEKtrRcRtDxIaxku3iHFXkBaobGi5tHgpxUDAyDIt/E2bvRShuMseOR7JSN2jzr1YrngxgW4PxyfMwJAZPuU/G564Mdxv7arI3T5L7nhLVAI4rLLGSEiPGYTP08G58fIieL1Rm8nABEWfviMFbUBb8w1yESgZY1vyyA18wJBAMSaOTxtqkEn2lJPO8GbDnZQ+WujVz6PWbJWDa8HMXqJxj7PtFs/HcTJNOeN6GOO/AB3jtBOq/bzbdBA8hXbVe0CQHp7hwxuuo+/3i6bHOeshbZNm7LxPaRfQCAQoObBMFUMQcm2Oyv5zqSoahgFO9ZxXe+6JPil6wd7fbGBuGp18aE='
,1

Union All
 
SELECT 
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDG0lXAsTO1+OHuB1OoWh4RReaYUwQVVAOdC+dz7+/gJtIjUo5ddiDfIS+0h6bJaIcUo+aUNV6eHnNPaXpx3/MITMyf87tNrEzzyu0cWK/BVoUzuiD+z9/aXBMQzCATzuggjL+xwBHPXL2Ju8iFfufqVWlMEREwhzMuURLgHBWj6QIBEQ=='
,'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAMbSVcCxM7X44e4HU6haHhFF5phTBBVUA50L53Pv7+Am0iNSjl12IN8hL7SHpslohxSj5pQ1Xp4ec09penHf8whMzJ/zu02sTPPK7RxYr8FWhTO6IP7P39pcExDMIBPO6CCMv7HAEc9cvYm7yIV+5+pVaUwRETCHMy5REuAcFaPpAgERAoGACjvEaesAx3vec8foEhOyfR/VR9cYrkY05FGNy51KdtTSWGgDkBrJOKk8zvBk0eVhTj0kVq/sZkGROqz1XHMOZBnbK3bC8RwLobeZnyCq6IfS2rvHAKNQfm7aQOWLoIfZckyiglFsR8ThXrCl401vRlTSgAzYoeSGmR4ouWTSCLECQQDwwF0p1OABFl948Mg3rS1HhfZDKNpAkBVugfFCIPiZiMjkd944y8w0VRbTCMsShb1hmwLMF+616OMWiKVUuVypAkEA02oa6xr8rubgrAqJplM3dv6jyQJhTuq+zQDE+a9+nN6q+/KB66CQyZb6EwQxHjlozPkjuRqvVMoaFSnfstlVQQJAOKW7kV9DxAVDo/xrWGT7mFvBeTbJ8RLX3cQ4xEP+Qj5NYu8HHGwwDFBBmxEguRBoy6wAqH4aDK9EfcXMjGfZkQJBAJU71sQTDLeyCAD4YSocn59KVX7UgOxpd5/ETs5dwskVpd8FiOKPdUMBKP5dT9kZdydGc5G4e8Nhe9K0JW8v//ECQAECI65lYQpn9jp0cGbn0fo9ctgNfL2UN9fg34op/k6RFpHjpp9ttrJlaTSKzHyvD97gZeAAryzZ8ekr86x/v2c='
,2

----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[Permission]
           ([PermID]
           ,[PermName]
           ,[PermDesc])

SELECT 1,'View','View Permission'
Union All
SELECT 3,'Add','Add ,View Permission'
Union All
SELECT 7,'Modify','Modify,Add & View Permission'
Union All
SELECT 15,'Delete','Delete, Modify,Add & View Permission'

----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[RootCAkey]
           ([id]
           ,[PublicKey]
           ,[PrivateKey])
SELECT 1
,'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQC0wAlroKdiinTVDXEIAXPTrf30M2En9K7uWwANrZ6y+04Rref0/ssMUy3UeUJZ1uRx+PPd35ZranTw0zEjaL8x4vP/3iObuRc+aek6hsgbBzq4EajEf60/dS9PV+n7uSn7vQ+it9K0iZTsjFrgmKeMh4ecqa4gS/IhBUJKt6gtawIBEQ=='
,'MIICcwIBADANBgkqhkiG9w0BAQEFAASCAl0wggJZAgEAAoGBALTACWugp2KKdNUNcQgBc9Ot/fQzYSf0ru5bAA2tnrL7ThGt5/T+ywxTLdR5QlnW5HH4893flmtqdPDTMSNovzHi8//eI5u5Fz5p6TqGyBsHOrgRqMR/rT91L09X6fu5Kfu9D6K30rSJlOyMWuCYp4yHh5ypriBL8iEFQkq3qC1rAgERAoGAL9h69tez1lHToc7aJ8QmMIDhVzrG5O3yEerpbQhPp9kcMds13vgmsHBe9HpzciJLh5S5BgZ6o/aIXd2NAdf2X5UcJfCr69WpB3F1WoZzSvPrLUmtVqkG2ToGag8HYRWX3BfupF1HrsbW1Bl+cCG+tbHBKRTPwJfUelTGNfpvvBECQQDGRTSKwIYrlIMSH0nuv0axNnO5FDEv1NDc89iTvBUM0nOZx64x9nJsnaWRKCBStDIbHYg0Vk7N/XuGmu2kcE2hAkEA6WDmqb0o2CCfOWPv8XG5LlLul5P2mGk59epX4LywzIueAcJrJXgfn5LQYf/DVLEdzOWyJrjBB3yed0jKC8qniwJAIv0nY8efNN35AzKysalItdxuxk7bgOlSCN+8zsbWicrJGyM826P2Eyrg+3+NO8VyQQU2J1qGYJYk24TAhm4r0QJAYBjXcxGnaA1uvUdErrZbT09TL1sLL7LboX6cp9U5vaLmtW4sHnzBui1k3Q72E9B1rrjvAOKp1egFA/DLqoCfVwJAHqRn5p0EhKdF6Z3TTHnunYddF8lwdBKywlvw65AbilnGOLJ9wA/tW0AhDujuEHExYRBHYAv6VdVCMxkCCe6a3g=='

----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[ServerInfo]
           ([ServerID]
           ,[ServerURI]
           ,[ServerPubKey]
           ,[ServerPrivKey]
           ,[ServerPortNo]
           ,[AdapterSrcPath]
           ,[ServerName])

SELECT '1','http://10.0.30.79/RequestHandler/Service.asmx'
,'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQCstEUBPdDvm0f8GYkz1FX91MjYZ14x4zO13+Z9LVAxQpl54vjoNvJQVZU3OzB9eANj5JwBXDDIFy2iVAHrldN0m4EpVIPi0/xU6V8iiPvb7HqN/4h/uoZUefqOnaAE1tlx3ah/m5etHBpxiQiIM+Umdjqz9FXUFNH4/afpRzsRmQIBEQ=='
,'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW2XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBnC5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2I1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bNB0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,null,null,null


----------------------------------------------------------------------------------
INSERT INTO [MobiusServer].[dbo].[FacilityPermissionInfo]
           ([FacilityId]
           ,[UserTypeId]
           ,[PubKey]
           ,[PrivKey]
           ,[IsShare])

SELECT (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') ,(select Usertypeid from userType where userType.TypeName='Administrator'),
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQCstEUBPdDvm0f8GYkz1FX91MjYZ14x4zO13+Z9LVAxQpl54vjoNvJQVZU3OzB9eANj5JwBXDDIFy2iVAHrldN0m4EpVIPi0/xU6V8iiPvb7HqN/4
h/uoZUefqOnaAE1tlx3ah/m5etHBpxiQiIM+Umdjqz9FXUFNH4/afpRzsRmQIBEQ==',
'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW2
XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBnC
5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2I
1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bNB
0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,1

Union All

SELECT (select facilityid from facilityinfo where name = 'MAMC'),(select Usertypeid from userType where TypeName='Technician'),
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDmQEZRUY/SobApnazsdLjvYNWGmaAo7Q0ffrFopSqiqG3zc36+qFAKNGap3l7e8J+xSXixSUutLCRYLfsyGwIeLnFNtZDv73kUbxfz3VXASs4U8tc5R
uqnrBjPUaS1GOJqPT+efgiN7bowHCCtoSeCG2imI0xqs5cPvD35l5u66QIBEQ==',
'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW2
XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBnC
5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2I
1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bNB
0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,0

Union All

SELECT (select facilityid from facilityinfo where name = 'MAMC'),(select Usertypeid from userType where TypeName='AHLTA Patient'),
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDmQEZRUY/SobApnazsdLjvYNWGmaAo7Q0ffrFopSqiqG3zc36+qFAKNGap3l7e8J+xSXixSUutLCRYLfsyGwIeLnFNtZDv73kUbxfz3VXASs4U8tc5RuqnrBjPUaS1GO
JqPT+efgiN7bowHCCtoSeCG2imI0xqs5cPvD35l5u66QIBEQ==',
'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW
2XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBn
C5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2
I1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bN
B0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,0


Union All

SELECT (select facilityid from facilityinfo where name = 'MAMC'),(select Usertypeid from userType where TypeName='Front Desk'),
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDmQEZRUY/SobApnazsdLjvYNWGmaAo7Q0ffrFopSqiqG3zc36+qFAKNGap3l7e8J+xSXixSUutLCRYLfsyGwIeLnFNtZDv73kUbxfz3VXASs4U8tc5RuqnrBjPUaS1GOJqPT
+efgiN7bowHCCtoSeCG2imI0xqs5cPvD35l5u66QIBEQ==',
'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW2
XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBnC
5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2I
1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bNB
0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,0

Union All

SELECT (select facilityid from facilityinfo where name = 'MAMC'),(select Usertypeid from userType where TypeName='Provider'),
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDmQEZRUY/SobApnazsdLjvYNWGmaAo7Q0ffrFopSqiqG3zc36+qFAKNGap3l7e8J+xSXixSUutLCRYLfsyGwIeLnFNtZDv73kUbxfz3VXASs4U8tc5Ruqn
rBjPUaS1GOJqPT+efgiN7bowHCCtoSeCG2imI0xqs5cPvD35l5u66QIBEQ==',
'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW2
XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBnC
5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2I
1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bNB
0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,0

Union All

SELECT (select facilityid from facilityinfo where name = 'Green County'),(select Usertypeid from userType where TypeName='OMF User'),
'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDmQEZRUY/SobApnazsdLjvYNWGmaAo7Q0ffrFopSqiqG3zc36+qFAKNGap3l7e8J+xSXixSUutLCRYLfsyGwIeLnFNtZDv73kUbxfz3VXASs4U8tc5RuqnrBjPUaS1GOJqPT
+efgiN7bowHCCtoSeCG2imI0xqs5cPvD35l5u66QIBEQ==',
'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW2
XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBnC
5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2I
1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bNB
0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,0

Union All

SELECT (select facilityid from facilityinfo where name = 'Green County'),(select Usertypeid from userType where TypeName='OMFAdministrator')
,'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDmQEZRUY/SobApnazsdLjvYNWGmaAo7Q0ffrFopSqiqG3zc36+qFAKNGap3l7e8J+xSXixSUutLCRYLfsyGwIeLnFNtZDv73kUbxfz3VXASs4
U8tc5RuqnrBjPUaS1GOJqPT+efgiN7bowHCCtoSeCG2imI0xqs5cPvD35l5u66QIBEQ==',
'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAKy0RQE90O+bR/wZiTPUVf3UyNhnXjHjM7Xf5n0tUDFCmXni+Og28lBVlTc7MH14A2PknAFcMMgXLaJUAeuV03SbgSlUg+LT/FTpXyKI+9vseo3/iH+6hlR5+o6doATW
2XHdqH+bl60cGnGJCIgz5SZ2OrP0VdQU0fj9p+lHOxGZAgERAoGABRRcYes62d7rh2oprrNr8OCcftXdH5W9wZWk5ZBkPa8iobPaJPKOqAKEY4G9qVZac+ETpa/jUS3bsfNpd97CdFJ8Dpg0KjxUjC8zwWfGgNSyENsihTmpWU0xB0aBn
C5Sky25GbQ6wlcxkHxrhRZL1WQAAB2CHNxy1j0mxqPfhNkCQQDqcppZqO0r/lHhUptIG/u14nDRz/3sxWEVrkl5BxE5Su7vZGvqTRD964JZaHAzlaQ8pHMr/qMsZj0accMc4S1NAkEAvJSew+1ZpMNkwyzXeILD9vHgEyLQJUMXI8lNx2
I1dpb2266pwXzMiZm+qVhrCj0q8ZY83Q9boStHxA/EZqo/fQJAKV+Epmkay4c7oDvBDLmk49yqf2DwkzHj9MRnUZfV3O8bGy/Wzv6KhymPeTCMRVakgywUUw7RgE5HBKqp9gmelQJBAJtNN3Qs4GmR2oKdZicCRweaA9OGFNNkbWjD5bN
B0az0y0uAyAjBIOnJ6E85wY/2I12KyLYMpdAFhmU6KUV9JTkCQA/qRebsSWw5lty/AWiWsuXyrbZty/Vfi8Mwp/dw7KgvqxEGjBbaHSaVzIyKnjRe3jF/QJURGYrxiMKe31HMH2c='
,0


----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[FacilityPerm]
           ([FacAssoID]
           ,[PermID]
           ,[ConsrtID]
           ,[Value])

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='Administrator'))
		,15,1,'7/1/2008 12:00:00 AM'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='Front Desk'))
,1,1,'7/1/2008 12:00:00 AM'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='Provider'))
,7,2,'8191'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'Green County') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='OMF User'))
,15,1,'7/1/2008 12:00:00 AM'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'Green County') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='OMFAdministrator'))
,7,2,'8191'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='AHLTA Patient'))
,1,2,'8191'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'Green County') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='OMF User'))
,1,2,'8191'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='Provider'))
,7,1,'7/1/2008 12:00:00 AM'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='Administrator'))
,15,2,'8191'
Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='Technician'))
,7,2,'8191'

Union All

SELECT (select FacAssoID from FacilityPermissionInfo where FacilityPermissionInfo.facilityid = (select facilityid from facilityinfo where facilityinfo.name = 'MAMC') 
		and  FacilityPermissionInfo.Usertypeid = (select Usertypeid from UserType where UserType.TypeName='Front Desk'))
,1,2,'8191'

----------------------------------------------------------------------------------


----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[UserInfo]
           ([UserID]
           ,[PassHash]
           ,[FirstName]
           ,[MidName]
           ,[LastName]
           ,[DOB]
           ,[SSN]
           ,[Email]
           ,[IsActive]
           ,[PubKey]
           ,[PrivKey]
           ,[UserTypeID]
           ,[FacilityID]
           ,[Nationality]
           ,[Force]
           ,[Sex]
           ,[UIC]
           ,[Religion]
           ,[FMP]
           ,[Race]
           ,[MOS]
           ,[Grade]
           ,[CanWorkOffline])
     
SELECT  'admin','admin','Administrator','','Admin','10/2/1980 12:00:00 AM','123456789'
,'admin@yahoo.com',1
,'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDks22N6aC0XOiSBs6ModbDv/yMAUNKHcEVOGimbjgdfi0/3t6kU6Ekd13V9mmBErf3r36wTi6vE+F8ryu12WhVoFSaZQMRu0Nvkfoap96MTmMqa9fQv+tkv6mIeZzvsP3lxHb67GXn
HIsTIvEYoHvr+j7JqJnMqhYlvNWhD+NPvQIBEQ=='
,'MIICdAIBADANBgkqhkiG9w0BAQEFAASCAl4wggJaAgEAAoGBAOSzbY3poLRc6JIGzoyh1sO//IwBQ0odwRU4aKZuOB1+LT/e3qRToSR3XdX2aYESt/evfrBOLq8T4XyvK7XZaFWgVJplAxG7Q2+R+hqn3oxOYypr19C/62S/qYh5nO+w
/eXEdvrsZeccixMi8Rige+v6PsmomcyqFiW81aEP40+9AgERAoGANc/dis2POX9FyAGaAvjnPR4dTh5qL46HyMH6gYNYf2j7lo7K+X0W22dhX4VGAECjwc7wofRHOEDpwvwKSOfcT/AVZHy1noWjuP4WLkvJheJGYlCX/Zm9xkHVpa5HM
p/EzEyKdKXi6LVNXgaQGYA5GhjUx3AI4nEs4b+BcYUefuECQQD1PmnSwyprCO+tBMQxicrhntm3MBWURH+GK+VuHHfi5H58CcJzcFOrDfEAKX/GpMHrkwN9ORTmXYWI6cZbBRVPAkEA7rtFgDwFmEKtrRcRtDxIaxku3iHFXkBaobGi5t
HgpxUDAyDIt/E2bvRShuMseOR7JSN2jzr1YrngxgW4PxyfMwJAZPuU/G564Mdxv7arI3T5L7nhLVAI4rLLGSEiPGYTP08G58fIieL1Rm8nABEWfviMFbUBb8w1yESgZY1vyyA18wJBAMSaOTxtqkEn2lJPO8GbDnZQ+WujVz6PWbJWDa8
HMXqJxj7PtFs/HcTJNOeN6GOO/AB3jtBOq/bzbdBA8hXbVe0CQHp7hwxuuo+/3i6bHOeshbZNm7LxPaRfQCAQoObBMFUMQcm2Oyv5zqSoahgFO9ZxXe+6JPil6wd7fbGBuGp18aE='
,(select UserTypeId from UserType where UserType.TypeName='Administrator' ),(select facilityid from facilityinfo where facilityinfo.name='MAMC'),'American','US 
Army','M','','','','','','',1

Union All

SELECT 'adminomf','Password3','new','admin','omf','7/31/1981 12:00:00 AM','321123456'
,'newadminomf@rsi.com',1
,'MIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQDIRu7XPBoyxw/QssBEgwkmDmX4X32JmdE6gj8S/ck4+EKj+TainXRTCuY/PnFD6b4mr/K9djqQ2PzqcbrXzPjnd11M0f2e9lVHaUjYk8n37qZBQErKI1q8D/HKk7dy13fLVYq+d1FsDg6EVcp6JR4jUTNjyyDj0IIs6gnCVXd49QIBEQ=='
,'MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAMhG7tc8GjLHD9CywESDCSYOZfhffYmZ0TqCPxL9yTj4QqP5NqKddFMK5j8+cUPpviav8r12OpDY/OpxutfM+Od3XUzR/Z72VUdpSNiTyffupkFASsojWrwP8cqTt3LXd8tVir53UWwODoRVynolHiNRM2PLIOPQgizqCcJVd3j1AgERAoGADJ9cf5T/f/kvP7U3JJY94zSJp1FP+rBqxDmwE3uO1VajZK/5w0HbOwPp8J/m3qIX0PWTmeyPhebz+UNnNFLVmk79T7Z9pvH6oGYDgAtDZgxJKRM8omlzwkjc+dLe0fVSE2YFD7KtfTZw2XJYpt4+QK385TzDUDi+2JYz5OVvmHcCQQDsLvj+HBsjajIcQjSYqKCQ76VFAyK6rRcYmswTilfG0cojgca3aUVvFr/n6Sk1AmnC26j55vtau4X3d233bcmPAkEA2RS4Yjn3wnOKtiRwFTg3Z5QyY6e4mzyrYi1gf27CZMLvifztm9IfpZLSzw1fsmIdtKsfL0ERvjbUCcFXv0ErOwJBALScZA2dBbGrj7tBr8AIeskvup4vkwc5EalnUMOmBuNVIhsnAV8URCfGGkfuiOxNI7MgciiDdOsH3upqYyafP8cCQCZO81yg0V6M3D5Cql4Y+rfs+dVZ1UiSPFyemI71bZlPk67wZiqOfg4o+AZrxZfzMmstQb0Le5oJrPKpphKxJb8CQQCiNgCvqIpQKiecVMDPX5ctJSaMakRIxwBpL61meNY9KgVXou+r6to7pbG1rXWE9Fu/9sMz+86fIg+X4looc4Ql'
,(select UserTypeId from UserType where UserType.TypeName='OMFAdministrator' ),(select facilityid from facilityinfo where facilityinfo.name='Green County'),'American','US Army','M','','','','','','',0


----------------------------------------------------------------------------------


INSERT INTO [MobiusServer].[dbo].[AdminUser]
           ([UserGUID]
           ,[Unit])
SELECT (SELECT UserGUID from UserInfo Where UserInfo.UserID = 'admin'),'Unit1'

----------------------------------------------------------------------------------

INSERT INTO [MobiusServer].[dbo].[OMFUser]
           ([UserGUID]
           ,[Initials]
           ,[Category])

SELECT (SELECT UserGUID from UserInfo Where UserInfo.UserID = 'adminomf'),'Initial1','Category1'




