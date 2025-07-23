package com.ada.sigorta.customer.dto;



import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class CustomerResponse {
    private Long id;
    private String nationalId;
    private String name;
    private String phone;
    private String address;
}

