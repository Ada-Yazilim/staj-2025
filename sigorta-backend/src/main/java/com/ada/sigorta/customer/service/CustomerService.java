package com.ada.sigorta.customer.service;



import com.ada.sigorta.customer.dto.CustomerRequest;
import com.ada.sigorta.customer.dto.CustomerResponse;
import com.ada.sigorta.customer.model.Customer;
import com.ada.sigorta.customer.repository.CustomerRepository;
import com.ada.sigorta.user.model.Users;
import com.ada.sigorta.user.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class CustomerService {

    private final CustomerRepository customerRepository;
    private final UserRepository userRepository;

    public void createOrUpdateCustomer(CustomerRequest request, Authentication authentication) {
        Users user = userRepository.findByUsername(authentication.getName()).orElseThrow();

        Customer customer = customerRepository.findByUser(user);
        if (customer == null) {
            customer = new Customer();
            customer.setUser(user);
        }

        customer.setNationalId(request.getNationalId());
        customer.setName(request.getName());
        customer.setPhone(request.getPhone());
        customer.setAddress(request.getAddress());

        customerRepository.save(customer);
    }

    public CustomerResponse getMyCustomerInfo(Authentication authentication) {
        Users user = userRepository.findByUsername(authentication.getName()).orElseThrow();
        Customer customer = customerRepository.findByUser(user);

        return CustomerResponse.builder()
                .id(customer.getId())
                .nationalId(customer.getNationalId())
                .name(customer.getName())
                .phone(customer.getPhone())
                .address(customer.getAddress())
                .build();
    }
}
