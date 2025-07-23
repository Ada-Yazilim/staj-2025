package com.ada.sigorta.customer.model;



import com.ada.sigorta.policy.model.Policy;
import com.ada.sigorta.user.model.Users;

import jakarta.persistence.*;
import lombok.*;

import java.util.List;

@Entity
@Table(name = "customers")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Customer {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String nationalId;  // TCKN/VKN
    private String name;
    private String address;
    private String phone;

    
    @OneToOne
    @JoinColumn(name = "user_id")
    private Users user;

    @OneToMany(mappedBy = "customer", cascade = CascadeType.ALL)
    private List<Policy> policies;
}

