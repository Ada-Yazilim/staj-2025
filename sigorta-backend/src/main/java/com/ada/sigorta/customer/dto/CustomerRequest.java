package com.ada.sigorta.customer.dto;



import lombok.Data;

@Data
public class CustomerRequest {
    private String nationalId;
    private String name;
    private String phone;
    private String address;
}

