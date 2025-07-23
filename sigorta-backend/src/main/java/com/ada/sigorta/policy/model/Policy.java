package com.ada.sigorta.policy.model;



import com.ada.sigorta.customer.model.Customer;
import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDate;

@Builder
@Entity
@Table(name = "policies")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Policy {


    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    
    @Enumerated(EnumType.STRING)
    private PolicyType type;

    
    private LocalDate startDate;
    private LocalDate endDate;

    
    private double premium;
    private boolean isPaid;

    
    @ManyToOne
    @JoinColumn(name = "customer_id")
    private Customer customer;
}

